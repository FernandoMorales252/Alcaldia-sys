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
    public class InventarioRepositoryTest
    {
        private readonly AppDbContext _context;
        private readonly InventarioRepository _sut;

        public InventarioRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"InventarioRepoTestDb_{Guid.NewGuid()}")
                .Options;

            _context = new AppDbContext(options);
            _sut = new InventarioRepository(_context);

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


            context.Inventarios.AddRange(new List<Inventario>
            {
                new Inventario
                {
                    Id_inventario= 1,
                    Nombre_item = "Silla",
                    Descripcion = "Ninguno",
                    Fecha_ingreso = new DateTime(2020, 1, 1),
                    Cantidad = 3,
                    Estado = "Uso",
                    MunicipioId = 20
                },
                new Inventario
                {
                      Id_inventario= 2,
                    Nombre_item = "Mesa",
                    Descripcion = "Ninguno",
                    Fecha_ingreso = new DateTime(2020, 1, 1),
                    Cantidad = 4,
                    Estado = "Dañado",
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
        public async Task GetAllAsync_DebeRetornarTodosLosItems()
        {
            var result = await _sut.GetAllAsync();

            Assert.NotNull(result);
            Assert.IsType<List<Inventario>>(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, e => e.Nombre_item == "Silla");
        }


        [Fact]
        public async Task GetByIdAsync_DebeRetornarDocumento_CuandoExiste()
        {
            var result = await _sut.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id_inventario);
            Assert.Equal("Silla", result.Nombre_item);
            Assert.Equal(20, result.MunicipioId);
        }

        [Fact]
        public async Task GetByIdAsync_DebeRetornarNull_CuandoNoExiste()
        {
            var result = await _sut.GetByIdAsync(99);
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_DebeAgregarItemYRetornarlo()
        {
            var nuevoItem = new Inventario
            {
                Nombre_item = "Laptop",
                Descripcion = "Ninguno",
                Fecha_ingreso = new DateTime(2020, 1, 1),
                Cantidad = 5,
                Estado = "Uso",
                MunicipioId = 20
            };

            var result = await _sut.AddAsync(nuevoItem);

            Assert.NotNull(result);
            Assert.True(result.Id_inventario > 0);
            Assert.Equal("Laptop", result.Nombre_item);

            var itemGuardado = await _context.Inventarios.FindAsync(result.Id_inventario);
            Assert.NotNull(itemGuardado);
            Assert.Equal("Uso", itemGuardado.Estado);
        }


        [Fact]
        public async Task UpdateAsync_DebeActualizarItemYRetornarTrue_CuandoExiste()
        {
            var ItemActualizar = await _context.Inventarios.FindAsync(1);
            ItemActualizar.Estado = "Dañado";
            ItemActualizar.MunicipioId = 10;

            var result = await _sut.UpdateAsync(ItemActualizar);

            Assert.True(result);

            var ItemActualizado = await _context.Inventarios.AsNoTracking().FirstOrDefaultAsync(e => e.Id_inventario == 1);
            Assert.Equal("Dañado", ItemActualizado!.Estado);
            Assert.Equal(10, ItemActualizado.MunicipioId);
        }

        [Fact]
        public async Task DeleteAsync_DebeEliminarItemYRetornarTrue_CuandoExiste()
        {
            int idAEliminar = 2;
            var result = await _sut.DeleteAsync(idAEliminar);
            Assert.True(result);

            Assert.Null(await _context.Inventarios.FindAsync(idAEliminar));
            Assert.Equal(1, await _context.Inventarios.CountAsync());
        }

        [Fact]
        public async Task DeleteAsync_DebeRetornarFalse_CuandoNoExiste()
        {
            var result = await _sut.DeleteAsync(99);

            Assert.False(result);
            Assert.Equal(2, await _context.Inventarios.CountAsync());
        }
    }
}
