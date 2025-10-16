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
    // Implementa IDisposable para asegurar que la base de datos en memoria se limpie
    public class EmpleadoRepositoryTest : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly EmpleadoRepository _sut; // System Under Test

        public EmpleadoRepositoryTest()
        {
            // --- ARRANGE GLOBAL: Configuración de la base de datos en memoria ---
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"EmpleadoRepoTestDb_{Guid.NewGuid()}")
                .Options;

            _context = new AppDbContext(options);
            _sut = new EmpleadoRepository(_context);

            SeedData(_context);
        }

        // Carga datos iniciales para las pruebas
        private void SeedData(AppDbContext context)
        {
            // 1. Sembrar Entidades Relacionadas (Cargo y Municipio)
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

            // 2. Sembrar Empleados
            context.Empleados.AddRange(new List<Empleado>
            {
                new Empleado
                {
                    Id_empleado = 1,
                    Nombre = "Juan",
                    Apellido = "Pérez",
                    Fecha_contratacion = new DateTime(2020, 1, 1),
                    Estado = "Activo",
                    CargoId = 1, // Alcalde
                    MunicipioId = 10 // San Salvador
                },
                new Empleado
                {
                    Id_empleado = 2,
                    Nombre = "María",
                    Apellido = "Gómez",
                    Fecha_contratacion = new DateTime(2021, 5, 15),
                    Estado = "Activo",
                    CargoId = 2, // Tesorero
                    MunicipioId = 20 // Santa Ana
                }
            });
            context.SaveChanges();
        }

        // Limpia la base de datos después de cada prueba
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        // -------------------------------------------------------------------
        // --- Pruebas para GetAllAsync
        // -------------------------------------------------------------------

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

        // -------------------------------------------------------------------
        // --- Pruebas para GetByIdAsync
        // -------------------------------------------------------------------

        [Fact]
        public async Task GetByIdAsync_DebeRetornarEmpleado_CuandoExiste()
        {
            // Act
            var result = await _sut.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id_empleado);
            Assert.Equal("Juan", result.Nombre);
            // Comprobamos que las FKs se hayan guardado
            Assert.Equal(1, result.CargoId);
        }

        [Fact]
        public async Task GetByIdAsync_DebeRetornarNull_CuandoNoExiste()
        {
            // Act
            var result = await _sut.GetByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        // -------------------------------------------------------------------
        // --- Pruebas para AddAsync
        // -------------------------------------------------------------------

        [Fact]
        public async Task AddAsync_DebeAgregarEmpleadoYRetornarlo()
        {
            // Arrange
            var nuevoEmpleado = new Empleado
            {
                Nombre = "Carlos",
                Apellido = "Díaz",
                Fecha_contratacion = new DateTime(2023, 10, 1),
                Estado = "Pendiente",
                CargoId = 1, // Alcalde
                MunicipioId = 10 // San Salvador
            };

            // Act
            var result = await _sut.AddAsync(nuevoEmpleado);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id_empleado > 0);
            Assert.Equal("Carlos", result.Nombre);

            // Verificar la persistencia: buscamos en el contexto
            var empleadoGuardado = await _context.Empleados.FindAsync(result.Id_empleado);
            Assert.NotNull(empleadoGuardado);
            Assert.Equal("Pendiente", empleadoGuardado.Estado);
        }

        // -------------------------------------------------------------------
        // --- Pruebas para UpdateAsync
        // -------------------------------------------------------------------

        [Fact]
        public async Task UpdateAsync_DebeActualizarEmpleadoYRetornarTrue_CuandoExiste()
        {
            // Arrange
            var empleadoAActualizar = await _context.Empleados.FindAsync(1);
            empleadoAActualizar.Estado = "Inactivo";
            empleadoAActualizar.CargoId = 2; // Cambia de Alcalde a Tesorero

            // Act
            var result = await _sut.UpdateAsync(empleadoAActualizar);

            // Assert
            Assert.True(result);

            // Verificar la actualización en la base de datos
            var empleadoActualizado = await _context.Empleados.AsNoTracking().FirstOrDefaultAsync(e => e.Id_empleado == 1);
            Assert.Equal("Inactivo", empleadoActualizado!.Estado);
            Assert.Equal(2, empleadoActualizado.CargoId);
        }

        // -------------------------------------------------------------------
        // --- Pruebas para DeleteAsync
        // -------------------------------------------------------------------

        [Fact]
        public async Task DeleteAsync_DebeEliminarEmpleadoYRetornarTrue_CuandoExiste()
        {
            // Arrange
            int idAEliminar = 2;

            // Act
            var result = await _sut.DeleteAsync(idAEliminar);

            // Assert
            Assert.True(result);

            // Verificar la eliminación en la base de datos
            Assert.Null(await _context.Empleados.FindAsync(idAEliminar));
            Assert.Equal(1, await _context.Empleados.CountAsync()); // Solo queda 1 empleado
        }

        [Fact]
        public async Task DeleteAsync_DebeRetornarFalse_CuandoNoExiste()
        {
            // Act
            var result = await _sut.DeleteAsync(99);

            // Assert
            Assert.False(result);
            // El conteo total de empleados no debe cambiar (sigue siendo 2)
            Assert.Equal(2, await _context.Empleados.CountAsync());
        }
    }
}