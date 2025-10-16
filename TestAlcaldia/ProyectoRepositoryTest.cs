using AlcaldiaApi.Datos;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Repositorios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAlcaldia
{
    public class ProyectoRepositoryTest
    {
        private readonly AppDbContext _context;
        private readonly ProyectoRepository _sut; // System Under Test

        public ProyectoRepositoryTest()
        {
            // --- ARRANGE GLOBAL: Configuración de la base de datos en memoria ---
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"ProyectoRepoTestDb_{Guid.NewGuid()}")
                .Options;

            _context = new AppDbContext(options);
            _sut = new ProyectoRepository(_context);

            SeedData(_context);
        }

        // Carga datos iniciales para las pruebas
        private void SeedData(AppDbContext context)
        {
            // 1. Sembrar Entidades Relacionadas (Cargo y Municipio)
            context.Municipios.AddRange(new List<Municipio>
            {
                new Municipio { Id_Municipio= 1, Nombre_Municipio = "Izalco"  },
                new Municipio { Id_Municipio = 2, Nombre_Municipio = "Nahuizalco"  }
            });

            
            context.SaveChanges();

            // 2. Sembrar Empleados
            context.Proyectos.AddRange(new List<Proyecto>
            {
                new Proyecto
                {
                    Id_proyecto = 1,
                    Nombre = "Escuela",
                    Descripcion = "Contrección de escuela ",
                    Fecha_inicio = new DateTime(2020, 1, 1),
                    Fecha_fin = new DateTime(2021, 1, 1),
                    Estado = "Activo",
                    Presupuesto = 50000.00m,
                    MunicipioId = 1 // San Salvador
                },
                new Proyecto
                {
                    Id_proyecto = 2,
                    Nombre = "Puente",
                    Descripcion = "Contrección de puente ",
                    Fecha_inicio = new DateTime(2021, 2, 1),
                    Fecha_fin = new DateTime(2022, 1, 1),
                    Estado = "Activo",
                    Presupuesto = 50000.00m,
                    MunicipioId = 2 // San Salvador
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
        public async Task GetAllAsync_DebeRetornarTodosLosProyectos()
        {
            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Proyecto>>(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, e => e.Nombre == "Escuela");
        }

        // -------------------------------------------------------------------
        // --- Pruebas para GetByIdAsync
        // -------------------------------------------------------------------

        [Fact]
        public async Task GetByIdAsync_DebeRetornarProyecto_CuandoExiste()
        {
            // Act
            var result = await _sut.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id_proyecto);
            Assert.Equal("Escuela", result.Nombre);
            // Comprobamos que las FKs se hayan guardado
            Assert.Equal(1, result.MunicipioId);
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
        public async Task AddAsync_DebeAgregarProyectoYRetornarlo()
        {
            // Arrange
            var nuevoProyecto = new Proyecto
            {
                
                Nombre = "Arboles",
                Descripcion = "Plantación de arboles ",
                Fecha_inicio = new DateTime(2021, 4, 5),
                Fecha_fin = new DateTime(2022, 6, 1),
                Estado = "Activo",
                Presupuesto = 50000.00m,
                MunicipioId = 2 // San Salvador
            };

            // Act
            var result = await _sut.AddAsync(nuevoProyecto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id_proyecto > 0);
            Assert.Equal("Arboles", result.Nombre);

            // Verificar la persistencia: buscamos en el contexto
            var ProyectoGuardado = await _context.Proyectos.FindAsync(result.Id_proyecto);
            Assert.NotNull(ProyectoGuardado);
            Assert.Equal("Activo", ProyectoGuardado.Estado);
        }

        // -------------------------------------------------------------------
        // --- Pruebas para UpdateAsync
        // -------------------------------------------------------------------

        [Fact]
        public async Task UpdateAsync_DebeActualizarProyectoYRetornarTrue_CuandoExiste()
        {
            // Arrange
            var ProyectoActualizar = await _context.Proyectos.FindAsync(1);
            ProyectoActualizar.Estado = "Inactivo";
            ProyectoActualizar.MunicipioId = 2; // Cambia de Alcalde a Tesorero

            // Act
            var result = await _sut.UpdateAsync(ProyectoActualizar);

            // Assert
            Assert.True(result);

            // Verificar la actualización en la base de datos
            var ProyectoActualizado = await _context.Proyectos.AsNoTracking().FirstOrDefaultAsync(e => e.Id_proyecto == 1);
            Assert.Equal("Inactivo", ProyectoActualizado!.Estado);
            Assert.Equal(2, ProyectoActualizado.MunicipioId);
        }

        // -------------------------------------------------------------------
        // --- Pruebas para DeleteAsync
        // -------------------------------------------------------------------

        [Fact]
        public async Task DeleteAsync_DebeEliminarProyectoYRetornarTrue_CuandoExiste()
        {
            // Arrange
            int idAEliminar = 2;

            // Act
            var result = await _sut.DeleteAsync(idAEliminar);

            // Assert
            Assert.True(result);

            // Verificar la eliminación en la base de datos
            Assert.Null(await _context.Proyectos.FindAsync(idAEliminar));
            Assert.Equal(1, await _context.Proyectos.CountAsync()); // Solo queda 1 empleado
        }

        [Fact]
        public async Task DeleteAsync_DebeRetornarFalse_CuandoNoExiste()
        {
            // Act
            var result = await _sut.DeleteAsync(99);

            // Assert
            Assert.False(result);
            // El conteo total de empleados no debe cambiar (sigue siendo 2)
            Assert.Equal(2, await _context.Proyectos.CountAsync());
        }
    }
}
