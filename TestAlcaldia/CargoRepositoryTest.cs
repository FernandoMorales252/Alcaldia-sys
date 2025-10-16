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
    // Implementa IDisposable para asegurar que la base de datos en memoria se limpie
    public class CargoRepositoryTest : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly CargoRepository _sut; // System Under Test

        public CargoRepositoryTest()
        {
            // --- ARRANGE GLOBAL: Configuración de la base de datos en memoria ---
            // Usamos un nombre único (Guid) para aislar la base de datos de cada prueba.
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"CargoRepoTestDb_{Guid.NewGuid()}")
                .Options;

            _context = new AppDbContext(options);
            _sut = new CargoRepository(_context);

            SeedData(_context);
        }

        // Carga datos iniciales para las pruebas de lectura, actualización y eliminación
        private void SeedData(AppDbContext context)
        {
            context.Cargos.AddRange(new List<Cargo>
            {
                new Cargo { Id_Cargo = 1, Nombre_cargo = "Alcalde", Descripcion = "Dirige el municipio" },
                new Cargo { Id_Cargo = 2, Nombre_cargo = "Secretario", Descripcion = "Asiste al alcalde" }
            });
            context.SaveChanges();
        }

        // Limpia la base de datos en memoria después de cada prueba
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        // -------------------------------------------------------------------
        // --- Pruebas para GetAllAsync
        // -------------------------------------------------------------------

        [Fact]
        public async Task GetAllAsync_DebeRetornarTodosLosCargos()
        {
            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Cargo>>(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Nombre_cargo == "Alcalde");
        }

        // -------------------------------------------------------------------
        // --- Pruebas para GetByIdAsync
        // -------------------------------------------------------------------

        [Fact]
        public async Task GetByIdAsync_DebeRetornarCargo_CuandoExiste()
        {
            // Act
            var result = await _sut.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id_Cargo);
            Assert.Equal("Alcalde", result.Nombre_cargo);
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
        public async Task AddAsync_DebeAgregarCargoYRetornarlo()
        {
            // Arrange
            var nuevoCargo = new Cargo { Nombre_cargo = "Tesorero", Descripcion = "Maneja dinero" };

            // Act
            var result = await _sut.AddAsync(nuevoCargo);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Tesorero", result.Nombre_cargo);
            // El Id se genera automáticamente o se asigna por la base de datos en memoria
            Assert.True(result.Id_Cargo > 0);

            // Verificar la persistencia: buscamos el cargo en el contexto
            var cargoGuardado = await _context.Cargos.FindAsync(result.Id_Cargo);
            Assert.NotNull(cargoGuardado);
            Assert.Equal("Maneja dinero", cargoGuardado.Descripcion);
        }

        // -------------------------------------------------------------------
        // --- Pruebas para UpdateAsync
        // -------------------------------------------------------------------

        [Fact]
        public async Task UpdateAsync_DebeActualizarCargoYRetornarTrue_CuandoExiste()
        {
            // Arrange
            var cargoAActualizar = await _context.Cargos.FindAsync(2);
            cargoAActualizar.Nombre_cargo = "Secretario Modificado";

            // Act
            var result = await _sut.UpdateAsync(cargoAActualizar);

            // Assert
            Assert.True(result);

            // Verificar la actualización en la base de datos
            var cargoActualizado = await _context.Cargos.AsNoTracking().FirstOrDefaultAsync(c => c.Id_Cargo == 2);
            Assert.Equal("Secretario Modificado", cargoActualizado.Nombre_cargo);
        }

        // -------------------------------------------------------------------
        // --- Pruebas para DeleteAsync
        // -------------------------------------------------------------------

        [Fact]
        public async Task DeleteAsync_DebeEliminarCargoYRetornarTrue_CuandoExiste()
        {
            // Arrange
            int idAEliminar = 1;

            // Act
            var result = await _sut.DeleteAsync(idAEliminar);

            // Assert
            Assert.True(result);

            // Verificar la eliminación en la base de datos
            Assert.Null(await _context.Cargos.FindAsync(idAEliminar));
            Assert.Equal(1, await _context.Cargos.CountAsync()); // Solo queda 1 cargo (el 2)
        }

        [Fact]
        public async Task DeleteAsync_DebeRetornarFalse_CuandoNoExiste()
        {
            // Act
            var result = await _sut.DeleteAsync(99);

            // Assert
            Assert.False(result);
            // El conteo total de cargos no debe cambiar (sigue siendo 2)
            Assert.Equal(2, await _context.Cargos.CountAsync());
        }
    }
}