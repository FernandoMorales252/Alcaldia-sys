using AlcaldiaApi.Datos;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Repositorios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAlcaldia;

namespace TestAlcaldia
{
    public class QuejaRepositoryTest
    {
        private readonly AppDbContext _context;
        private readonly QuejaRepository _sut; // System Under Test

        public QuejaRepositoryTest()
        {
            // --- ARRANGE GLOBAL: Configuración de la base de datos en memoria ---
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"QuejaRepoTestDb_{Guid.NewGuid()}")
                .Options;

            _context = new AppDbContext(options);
            _sut = new QuejaRepository(_context);

            SeedData(_context);
        }

        // Carga datos iniciales para las pruebas
        private void SeedData(AppDbContext context)
        {
            // 1. Sembrar Entidades Relacionadas (Cargo y Municipio)

            context.Municipios.AddRange(new List<Municipio>
            {
                new Municipio { Id_Municipio = 10, Nombre_Municipio = "San Salvador" },
                new Municipio { Id_Municipio = 20, Nombre_Municipio = "Santa Ana" }
            });
            context.SaveChanges();

            // 2. Sembrar Empleados
            context.Quejas.AddRange(new List<Queja>
            {
                new Queja
                {
                    Id_queja = 1,
                    Titulo = "Queja1",
                    Descripcion = "Motivo",
                    Fecha_Registro = new DateTime(2020, 1, 1),
                    Tipo = "Seguridad",
                    Nivel = "Alto",
                    MunicipioId = 10 // San Salvador
                },
                new Queja
                {
                   Id_queja = 2,
                    Titulo = "Queja2",
                    Descripcion = "Motivo2",
                    Fecha_Registro = new DateTime(2020, 1, 1),
                    Tipo = "Desechos",
                    Nivel = "Media",
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
        public async Task GetAllAsync_DebeRetornarTodasLasQuejas()
        {
            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Queja>>(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, e => e.Titulo == "Queja1");
        }

        // -------------------------------------------------------------------
        // --- Pruebas para GetByIdAsync
        // -------------------------------------------------------------------

        [Fact]
        public async Task GetByIdAsync_DebeRetornarQueja_CuandoExiste()
        {
            // Act
            var result = await _sut.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id_queja);
            Assert.Equal("Queja1", result.Titulo);
            // Comprobamos que las FKs se hayan guardado
            Assert.Equal(10, result.MunicipioId);
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
        public async Task AddAsync_DebeAgregarQuejaYRetornarlo()
        {
            // Arrange
            var nuevaQueja = new Queja
            {
                Titulo = "Queja3",
                Descripcion = "Motivo3",
                Fecha_Registro = new DateTime(2020, 1, 1),
                Tipo = "Desechos",
                Nivel = "Media",
                MunicipioId = 20 // Santa Ana
            };

            // Act
            var result = await _sut.AddAsync(nuevaQueja);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id_queja > 0);
            Assert.Equal("Queja3", result.Titulo);

            // Verificar la persistencia: buscamos en el contexto
            var quejaGuardado = await _context.Quejas.FindAsync(result.Id_queja);
            Assert.NotNull(quejaGuardado);
            Assert.Equal("Desechos", quejaGuardado.Tipo);
        }

        // -------------------------------------------------------------------
        // --- Pruebas para UpdateAsync
        // -------------------------------------------------------------------

        [Fact]
        public async Task UpdateAsync_DebeActualizarQuejaYRetornarTrue_CuandoExiste()
        {
            // Arrange
            var quejaActualizar = await _context.Quejas.FindAsync(1);
            quejaActualizar.Tipo = "Bajo";
            quejaActualizar.MunicipioId = 10;

            // Act
            var result = await _sut.UpdateAsync(quejaActualizar);

            // Assert
            Assert.True(result);

            // Verificar la actualización en la base de datos
            var quejaActualizado = await _context.Quejas.AsNoTracking().FirstOrDefaultAsync(e => e.Id_queja == 1);
            Assert.Equal("Bajo", quejaActualizado!.Tipo);
            Assert.Equal(10, quejaActualizado.MunicipioId);
        }

        // -------------------------------------------------------------------
        // --- Pruebas para DeleteAsync
        // -------------------------------------------------------------------

        [Fact]
        public async Task DeleteAsync_DebeEliminarQuejaYRetornarTrue_CuandoExiste()
        {
            // Arrange
            int idAEliminar = 2;

            // Act
            var result = await _sut.DeleteAsync(idAEliminar);

            // Assert
            Assert.True(result);

            // Verificar la eliminación en la base de datos
            Assert.Null(await _context.Quejas.FindAsync(idAEliminar));
            Assert.Equal(1, await _context.Quejas.CountAsync()); // Solo queda 1 empleado
        }

        [Fact]
        public async Task DeleteAsync_DebeRetornarFalse_CuandoNoExiste()
        {
            // Act
            var result = await _sut.DeleteAsync(99);

            // Assert
            Assert.False(result);
            // El conteo total de empleados no debe cambiar (sigue siendo 2)
            Assert.Equal(2, await _context.Quejas.CountAsync());


        }
    }
}
