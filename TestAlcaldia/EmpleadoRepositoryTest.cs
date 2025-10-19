using AlcaldiaApi.Datos;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Repositorios;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TestAlcaldia
{
    
    public class EmpleadoRepositoryTest : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly EmpleadoRepository _sut; 

        public EmpleadoRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoRepoTestDb_{Guid.NewGuid()}")
                .Options;

            _context = new AppDbContext(options);
            _sut = new EmpleadoRepository(_context);

            SeedData(_context);
        }

        private void SeedData(AppDbContext context)
        {
            context.Cargos.AddRange(new List<Cargo>
            {
                new Cargo { Id_Cargo = 1, Nombre_cargo = "Alcalde", Descripcion = "Líder" },
                new Cargo { Id_Cargo = 2, Nombre_cargo = "Tesorero", Descripcion = "Finanzas" }
            });

            context.Municipios.AddRange(new List<Municipio>
            {
                new Municipio { Id_Municipio = 10, Nombre_Municipio = "San Salvador" },
                new Municipio { Id_Municipio = 20, Nombre_Municipio = "Santa Ana" }
            });
            context.SaveChanges();

            
            context.Empleados.AddRange(new List<Empleado>
            {
                new Empleado
                {
                    Id_empleado = 1,
                    Nombre = "Juan",
                    Apellido = "Pérez",
                    Fecha_contratacion = new DateTime(2020, 1, 1),
                    Estado = "Activo",
                    CargoId = 1, 
                    MunicipioId = 10 
                },
                new Empleado
                {
                    Id_empleado = 2,
                    Nombre = "María",
                    Apellido = "Gómez",
                    Fecha_contratacion = new DateTime(2021, 5, 15),
                    Estado = "Activo",
                    CargoId = 2, 
                    MunicipioId = 20 
                }
            });
            context.SaveChanges();
        }

        
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

       
        [Fact]
        public async Task GetAllAsync_DebeRetornarTodosLosEmpleados()
        {
            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Empleado>>(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, e => e.Nombre == "Juan");
        }

        
        [Fact]
        public async Task GetByIdAsync_DebeRetornarEmpleado_CuandoExiste()
        {
            var result = await _sut.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id_empleado);
            Assert.Equal("Juan", result.Nombre);
            Assert.Equal(1, result.CargoId);
        }

        [Fact]
        public async Task GetByIdAsync_DebeRetornarNull_CuandoNoExiste()
        {
            var result = await _sut.GetByIdAsync(99);
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_DebeAgregarEmpleadoYRetornarlo()
        {
            var nuevoEmpleado = new Empleado
            {
                Nombre = "Carlos",
                Apellido = "Díaz",
                Fecha_contratacion = new DateTime(2023, 10, 1),
                Estado = "Pendiente",
                CargoId = 1, 
                MunicipioId = 10 
            };

            var result = await _sut.AddAsync(nuevoEmpleado);

            Assert.NotNull(result);
            Assert.True(result.Id_empleado > 0);
            Assert.Equal("Carlos", result.Nombre);

            var empleadoGuardado = await _context.Empleados.FindAsync(result.Id_empleado);
            Assert.NotNull(empleadoGuardado);
            Assert.Equal("Pendiente", empleadoGuardado.Estado);
        }


        [Fact]
        public async Task UpdateAsync_DebeActualizarEmpleadoYRetornarTrue_CuandoExiste()
        {
            var empleadoAActualizar = await _context.Empleados.FindAsync(1);
            empleadoAActualizar.Estado = "Inactivo";
            empleadoAActualizar.CargoId = 2; 

            var result = await _sut.UpdateAsync(empleadoAActualizar);

            Assert.True(result);

            var empleadoActualizado = await _context.Empleados.AsNoTracking().FirstOrDefaultAsync(e => e.Id_empleado == 1);
            Assert.Equal("Inactivo", empleadoActualizado!.Estado);
            Assert.Equal(2, empleadoActualizado.CargoId);
        }

        [Fact]
        public async Task DeleteAsync_DebeEliminarEmpleadoYRetornarTrue_CuandoExiste()
        {
            int idAEliminar = 2;
            var result = await _sut.DeleteAsync(idAEliminar);
            Assert.True(result);
           
            Assert.Null(await _context.Empleados.FindAsync(idAEliminar));
            Assert.Equal(1, await _context.Empleados.CountAsync()); 
        }

        [Fact]
        public async Task DeleteAsync_DebeRetornarFalse_CuandoNoExiste()
        {
            var result = await _sut.DeleteAsync(99);

            Assert.False(result);
            Assert.Equal(2, await _context.Empleados.CountAsync());
        }
    }
}