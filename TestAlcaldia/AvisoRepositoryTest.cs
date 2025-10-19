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
    public class AvisoRepositoryTest
    {
        private readonly AppDbContext _context;
        private readonly AvisoRepository _sut;

        public AvisoRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"AvisoRepoTestDb_{Guid.NewGuid()}")
                .Options;

            _context = new AppDbContext(options);
            _sut = new AvisoRepository(_context);

            SeedData(_context);
        }

        private void SeedData(AppDbContext context)
        {

            context.Municipios.AddRange(new List<Municipio>
            {
                new Municipio { Id_Municipio = 10, Nombre_Municipio = "San Salvador" },
                new Municipio { Id_Municipio = 20, Nombre_Municipio = "Santa Ana" }
            });
            context.SaveChanges();


            context.Avisos.AddRange(new List<Aviso>
            {
                new Aviso
                {
                    Id_aviso= 1,
                    Titulo = "Derrumbe",
                    Descripcion = "Ninguno",
                    Fecha_Registro = new DateTime(2020, 1, 1),
                    Tipo = "Climatico",
                    MunicipioId = 20
                },
                new Aviso
                {
                    Id_aviso= 2,
                    Titulo = "Fiesta",
                    Descripcion = "Ninguno",
                    Fecha_Registro = new DateTime(2020, 1, 1),
                    Tipo = "Festivo",
                    MunicipioId = 10
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
        public async Task GetAllAsync_DebeRetornarTodosLosAvisos()
        {
            var result = await _sut.GetAllAsync();

            Assert.NotNull(result);
            Assert.IsType<List<Aviso>>(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, e => e.Titulo == "Derrumbe");
        }


        [Fact]
        public async Task GetByIdAsync_DebeRetornarAviso_CuandoExiste()
        {
            var result = await _sut.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id_aviso);
            Assert.Equal("Derrumbe", result.Titulo);
            Assert.Equal(20, result.MunicipioId);
        }

        [Fact]
        public async Task GetByIdAsync_DebeRetornarNull_CuandoNoExiste()
        {
            var result = await _sut.GetByIdAsync(99);
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_DebeAgregarAvisoYRetornarlo()
        {
            var nuevoAviso = new Aviso
            {
                Titulo = "Salud",
                Descripcion = "Ninguno",
                Fecha_Registro = new DateTime(2020, 1, 1),
                Tipo = "Evento",
                MunicipioId = 10
            };

            var result = await _sut.AddAsync(nuevoAviso);

            Assert.NotNull(result);
            Assert.True(result.Id_aviso > 0);
            Assert.Equal("Salud", result.Titulo);

            var AvisoGuardado = await _context.Avisos.FindAsync(result.Id_aviso);
            Assert.NotNull(AvisoGuardado);
            Assert.Equal("Evento", AvisoGuardado.Tipo);
        }


        [Fact]
        public async Task UpdateAsync_DebeActualizarAvisoYRetornarTrue_CuandoExiste()
        {
            var AvisoActualizar = await _context.Avisos.FindAsync(1);
            AvisoActualizar.Tipo = "Catastrofe";
            AvisoActualizar.MunicipioId = 10;

            var result = await _sut.UpdateAsync(AvisoActualizar);

            Assert.True(result);

            var ItemActualizado = await _context.Avisos.AsNoTracking().FirstOrDefaultAsync(e => e.Id_aviso == 1);
            Assert.Equal("Catastrofe", ItemActualizado!.Tipo);
            Assert.Equal(10, ItemActualizado.MunicipioId);
        }

        [Fact]
        public async Task DeleteAsync_DebeEliminarAvisoYRetornarTrue_CuandoExiste()
        {
            int idAEliminar = 2;
            var result = await _sut.DeleteAsync(idAEliminar);
            Assert.True(result);

            Assert.Null(await _context.Avisos.FindAsync(idAEliminar));
            Assert.Equal(1, await _context.Avisos.CountAsync());
        }

        [Fact]
        public async Task DeleteAsync_DebeRetornarFalse_CuandoNoExiste()
        {
            var result = await _sut.DeleteAsync(99);

            Assert.False(result);
            Assert.Equal(2, await _context.Avisos.CountAsync());
        }
    }
}
