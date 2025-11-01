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
    
    public class MunicipioRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly MunicipioRepository _sut; 

        public MunicipioRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"MunicipioRepoTestDb_{Guid.NewGuid()}")
                .Options;

            _context = new AppDbContext(options);
            _sut = new MunicipioRepository(_context);

            SeedData(_context);
        }

        private void SeedData(AppDbContext context)
        {
            context.Municipios.AddRange(new List<Municipio>
            {
                new Municipio { Id_Municipio = 1, Nombre_Municipio = "San Salvador" },
                new Municipio { Id_Municipio = 2, Nombre_Municipio = "Antiguo Cuscatlán" }
            });
            context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


        [Fact]
        public async Task GetAllAsync_DebeRetornarTodosLosMunicipios()
        {
            var result = await _sut.GetAllAsync();

            Assert.NotNull(result);
            Assert.IsType<List<Municipio>>(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, m => m.Nombre_Municipio == "San Salvador");
        }

        [Fact]
        public async Task GetByIdAsync_DebeRetornarMunicipio_CuandoExiste()
        {
            var result = await _sut.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id_Municipio);
            Assert.Equal("San Salvador", result.Nombre_Municipio);
        }

        [Fact]
        public async Task GetByIdAsync_DebeRetornarNull_CuandoNoExiste()
        {
            var result = await _sut.GetByIdAsync(99);

            Assert.Null(result);
        }


        [Fact]
        public async Task AddAsync_DebeAgregarMunicipioYRetornarlo()
        {
            var nuevoMunicipio = new Municipio { Nombre_Municipio = "Santa Tecla" };

            var result = await _sut.AddAsync(nuevoMunicipio);

            Assert.NotNull(result);
            Assert.Equal("Santa Tecla", result.Nombre_Municipio);
            Assert.True(result.Id_Municipio > 0);

            var municipioGuardado = await _context.Municipios.FindAsync(result.Id_Municipio);
            Assert.NotNull(municipioGuardado);
            Assert.Equal(3, await _context.Municipios.CountAsync());
        }


        [Fact]
        public async Task UpdateAsync_DebeActualizarMunicipioYRetornarTrue()
        {
            var municipioAActualizar = await _context.Municipios.FindAsync(2);
            municipioAActualizar!.Nombre_Municipio = "Antiguo Cuscatlán Modificado";

            var result = await _sut.UpdateAsync(municipioAActualizar);

            Assert.True(result);

            var municipioActualizado = await _context.Municipios
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id_Municipio == 2);

            Assert.Equal("Antiguo Cuscatlán Modificado", municipioActualizado!.Nombre_Municipio);
        }


        [Fact]
        public async Task DeleteAsync_DebeEliminarMunicipioYRetornarTrue_CuandoExiste()
        {
            int idAEliminar = 1;

            var result = await _sut.DeleteAsync(idAEliminar);

            Assert.True(result);

            Assert.Null(await _context.Municipios.FindAsync(idAEliminar));
            Assert.Equal(1, await _context.Municipios.CountAsync());
        }

        [Fact]
        public async Task DeleteAsync_DebeRetornarFalse_CuandoNoExiste()
        {
            var result = await _sut.DeleteAsync(99);

            Assert.False(result);
            Assert.Equal(2, await _context.Municipios.CountAsync());
        }
    }
}
