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
    public class DocumentoRepositoryTest
    {
        private readonly AppDbContext _context;
        private readonly DocumentoRepository _sut;

        public DocumentoRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"DocumentoRepoTestDb_{Guid.NewGuid()}")
                .Options;

            _context = new AppDbContext(options);
            _sut = new DocumentoRepository(_context);

            SeedData(_context);
        }

        private void SeedData(AppDbContext context)
        {
            context.Tipos.AddRange(new List<TipoDocumento>
            {
                new TipoDocumento { Id_tipo = 1, Nombre= "Partida" },
                new TipoDocumento { Id_tipo = 2, Nombre = "Acta" }
            });

            context.Municipios.AddRange(new List<Municipio>
            {
                new Municipio { Id_Municipio = 10, Nombre_Municipio = "San Salvador" },
                new Municipio { Id_Municipio = 20, Nombre_Municipio = "Santa Ana" }
            });
            context.SaveChanges();


            context.Documentos.AddRange(new List<Documento>
            {
                new Documento
                {
                    Id_documento= 1,
                    Numero_documento = "001",
                    Propietario = "Paola",
                    Fecha_emision = new DateTime(2020, 1, 1),
                    Detalles = "Ninguno",
                    Estado = "Activo",
                    TipoDocumentoId = 1,
                    MunicipioId = 20
                },
                new Documento
                {
                     Id_documento= 2,
                    Numero_documento = "002",
                    Propietario = "Paola",
                    Fecha_emision = new DateTime(2020, 1, 1),
                    Detalles = "Ninguno",
                    Estado = "Activo",
                    TipoDocumentoId = 2,
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
        public async Task GetAllAsync_DebeRetornarTodosLosDocumentos()
        {
            var result = await _sut.GetAllAsync();

            Assert.NotNull(result);
            Assert.IsType<List<Documento>>(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, e => e.Numero_documento == "001");
        }


        [Fact]
        public async Task GetByIdAsync_DebeRetornarDocumento_CuandoExiste()
        {
            var result = await _sut.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id_documento);
            Assert.Equal("001", result.Numero_documento);
            Assert.Equal(1, result.TipoDocumentoId);
        }

        [Fact]
        public async Task GetByIdAsync_DebeRetornarNull_CuandoNoExiste()
        {
            var result = await _sut.GetByIdAsync(99);
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_DebeAgregarDocumentoYRetornarlo()
        {
            var nuevoDocumento = new Documento
            {
                Numero_documento = "003",
                Propietario = "Nicole",
                Fecha_emision = new DateTime(2020, 1, 1),
                Detalles = "Ninguno",
                Estado = "Inactivo",
                TipoDocumentoId = 2,
                MunicipioId = 10
            };

            var result = await _sut.AddAsync(nuevoDocumento);

            Assert.NotNull(result);
            Assert.True(result.Id_documento > 0);
            Assert.Equal("003", result.Numero_documento);

            var documentoGuardado = await _context.Documentos.FindAsync(result.Id_documento);
            Assert.NotNull(documentoGuardado);
            Assert.Equal("Inactivo", documentoGuardado.Estado);
        }


        [Fact]
        public async Task UpdateAsync_DebeActualizarDocumentoYRetornarTrue_CuandoExiste()
        {
            var documentoActualizar = await _context.Documentos.FindAsync(1);
            documentoActualizar.Estado = "Inactivo";
            documentoActualizar.TipoDocumentoId = 2;

            var result = await _sut.UpdateAsync(documentoActualizar);

            Assert.True(result);

            var documentoActualizado = await _context.Documentos.AsNoTracking().FirstOrDefaultAsync(e => e.Id_documento == 1);
            Assert.Equal("Inactivo", documentoActualizado!.Estado);
            Assert.Equal(2, documentoActualizado.TipoDocumentoId);
        }

        [Fact]
        public async Task DeleteAsync_DebeEliminarDocumentoYRetornarTrue_CuandoExiste()
        {
            int idAEliminar = 2;
            var result = await _sut.DeleteAsync(idAEliminar);
            Assert.True(result);

            Assert.Null(await _context.Documentos.FindAsync(idAEliminar));
            Assert.Equal(1, await _context.Documentos.CountAsync());
        }

        [Fact]
        public async Task DeleteAsync_DebeRetornarFalse_CuandoNoExiste()
        {
            var result = await _sut.DeleteAsync(99);

            Assert.False(result);
            Assert.Equal(2, await _context.Documentos.CountAsync());
        }
    }
}
