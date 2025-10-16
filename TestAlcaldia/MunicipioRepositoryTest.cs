using AlcaldiaApi.Datos;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Repositorios;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AlcaldiaApi.Tests.Unit.Repositories
{
    // Clase de pruebas para la capa de Repositorio de Municipio.
    // Implementa IDisposable para asegurar la limpieza del contexto en memoria.
    public class MunicipioRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly MunicipioRepository _sut; // System Under Test

        public MunicipioRepositoryTests()
        {
            // --- ARRANGE GLOBAL: Configuración de la base de datos en memoria ---
            // Usamos un nombre único (Guid) para aislar la base de datos de cada ejecución
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"MunicipioRepoTestDb_{Guid.NewGuid()}")
                .Options;

            _context = new AppDbContext(options);
            _sut = new MunicipioRepository(_context);

            SeedData(_context);
        }

        // Carga datos iniciales para las pruebas de lectura, actualización y eliminación
        private void SeedData(AppDbContext context)
        {
            context.Municipios.AddRange(new List<Municipio>
            {
                new Municipio { Id_Municipio = 1, Nombre_Municipio = "San Salvador" },
                new Municipio { Id_Municipio = 2, Nombre_Municipio = "Antiguo Cuscatlán" }
            });
            context.SaveChanges();
        }

        // Limpia y elimina la base de datos en memoria después de cada conjunto de pruebas
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        // ===================================================================
        // --- PRUEBAS DE LECTURA (Read) ---
        // ===================================================================

        [Fact]
        public async Task GetAllAsync_DebeRetornarTodosLosMunicipios()
        {
            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Municipio>>(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, m => m.Nombre_Municipio == "San Salvador");
        }

        [Fact]
        public async Task GetByIdAsync_DebeRetornarMunicipio_CuandoExiste()
        {
            // Act
            var result = await _sut.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id_Municipio);
            Assert.Equal("San Salvador", result.Nombre_Municipio);
        }

        [Fact]
        public async Task GetByIdAsync_DebeRetornarNull_CuandoNoExiste()
        {
            // Act
            var result = await _sut.GetByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        // ===================================================================
        // --- PRUEBAS DE CREACIÓN (Add) ---
        // ===================================================================

        [Fact]
        public async Task AddAsync_DebeAgregarMunicipioYRetornarlo()
        {
            // Arrange
            var nuevoMunicipio = new Municipio { Nombre_Municipio = "Santa Tecla" };

            // Act
            var result = await _sut.AddAsync(nuevoMunicipio);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Santa Tecla", result.Nombre_Municipio);
            // Verificar que la BD en memoria generó un Id. El conteo anterior era 2.
            Assert.True(result.Id_Municipio > 0);

            // Verificar la persistencia: buscamos el municipio directamente en el contexto
            var municipioGuardado = await _context.Municipios.FindAsync(result.Id_Municipio);
            Assert.NotNull(municipioGuardado);
            Assert.Equal(3, await _context.Municipios.CountAsync());
        }

        // ===================================================================
        // --- PRUEBAS DE ACTUALIZACIÓN (Update) ---
        // ===================================================================

        [Fact]
        public async Task UpdateAsync_DebeActualizarMunicipioYRetornarTrue()
        {
            // Arrange
            // Obtener una entidad existente (Id=2) para adjuntarla y modificarla
            var municipioAActualizar = await _context.Municipios.FindAsync(2);
            municipioAActualizar!.Nombre_Municipio = "Antiguo Cuscatlán Modificado";

            // Act
            var result = await _sut.UpdateAsync(municipioAActualizar);

            // Assert
            Assert.True(result);

            // Verificar la actualización en la base de datos
            var municipioActualizado = await _context.Municipios
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id_Municipio == 2);

            Assert.Equal("Antiguo Cuscatlán Modificado", municipioActualizado!.Nombre_Municipio);
        }

        // ===================================================================
        // --- PRUEBAS DE ELIMINACIÓN (Delete) ---
        // ===================================================================

        [Fact]
        public async Task DeleteAsync_DebeEliminarMunicipioYRetornarTrue_CuandoExiste()
        {
            // Arrange
            int idAEliminar = 1;

            // Act
            var result = await _sut.DeleteAsync(idAEliminar);

            // Assert
            Assert.True(result);

            // Verificar la eliminación en la base de datos
            Assert.Null(await _context.Municipios.FindAsync(idAEliminar));
            // El conteo debe ser 1 (solo queda el municipio con Id=2)
            Assert.Equal(1, await _context.Municipios.CountAsync());
        }

        [Fact]
        public async Task DeleteAsync_DebeRetornarFalse_CuandoNoExiste()
        {
            // Act
            var result = await _sut.DeleteAsync(99);

            // Assert
            Assert.False(result);
            // El conteo total de municipios no debe cambiar (sigue siendo 2)
            Assert.Equal(2, await _context.Municipios.CountAsync());
        }
    }
}
