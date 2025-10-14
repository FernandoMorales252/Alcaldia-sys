using Xunit;
using Moq;
using FluentAssertions;
using AlcaldiaApi.Interfaces;
using AlcaldiaApi.Servicios;
using AlcaldiaApi.DtOs.TipoDocumentoDTOs;
using AlcaldiaApi.Entidades;

namespace AlcaldiaApi.Tests.Unit.Services
{
    public class TipoDocServiceTests
    {
        private readonly Mock<ITipoDocRepository> _mockRepo = new();
        private readonly TipoDocService _sut; // System Under Test

        // --- ARRANGE GLOBAL ---
        public TipoDocServiceTests()
        {
            _sut = new TipoDocService(_mockRepo.Object);
        }

        private TipoDocumento GetTestEntity(int id = 1, string nombre = "Cédula") =>
            new TipoDocumento { Id_tipo = id, Nombre = nombre };


        // -----------------------------------------------------------------
        // --- PRUEBAS DE LECTURA (Read) ---
        // -----------------------------------------------------------------

        [Fact]
        public async Task GetAllAsync_ReturnsList_WhenItemsExist()
        {
            // Arrange
            var entities = new List<TipoDocumento> { GetTestEntity() };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Should().BeOfType<List<TipoDocumentoRespuestaDTO>>();
            result.Should().HaveCount(1);
            result.First().Nombre.Should().Be("Cédula");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TipoDocumento)null);

            // Act
            var result = await _sut.GetByIdAsync(99);

            // Assert
            result.Should().BeNull();
        }

        // -----------------------------------------------------------------
        // --- PRUEBAS DE CREACIÓN (Create) ---
        // -----------------------------------------------------------------

        [Fact]
        public async Task CreateAsync_CallsRepoAdd_AndReturnsDTO()
        {
            // Arrange
            var createDto = new TipoDocumentoCrearDTO { Nombre = "Pasaporte" };
            var savedEntity = GetTestEntity(id: 5, nombre: "Pasaporte");

            // Configurar el mock para devolver una entidad con el Id generado
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<TipoDocumento>())).ReturnsAsync(savedEntity);

            // Act
            var result = await _sut.CreateAsync(createDto);

            // Assert
            result.Should().BeOfType<TipoDocumentoRespuestaDTO>();
            result.Id_tipo.Should().Be(5);

            // Verificar que el repositorio fue llamado exactamente una vez con la entidad
            _mockRepo.Verify(r => r.AddAsync(It.Is<TipoDocumento>(e => e.Nombre == "Pasaporte")), Times.Once);
        }

        // -----------------------------------------------------------------
        // --- PRUEBAS DE ACTUALIZACIÓN (Update) ---
        // -----------------------------------------------------------------

        [Fact]
        public async Task UpdateAsync_ReturnsTrue_WhenSuccessful()
        {
            // Arrange
            var updateDto = new TipoDocumentoActualizarDTO { Nombre = "Cédula Actualizada" };
            var currentEntity = GetTestEntity(id: 1, nombre: "Cédula Vieja");

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(currentEntity);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<TipoDocumento>())).ReturnsAsync(true); // Simular éxito

            // Act
            var result = await _sut.UpdateAsync(1, updateDto);

            // Assert
            result.Should().BeTrue();

            // Verificar que la entidad se actualizó en el repositorio
            _mockRepo.Verify(r => r.UpdateAsync(It.Is<TipoDocumento>(e => e.Nombre == "Cédula Actualizada")), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenNotFound()
        {
            // Arrange
            var updateDto = new TipoDocumentoActualizarDTO { Nombre = "No existe" };
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((TipoDocumento)null); // Simular no encontrado

            // Act
            var result = await _sut.UpdateAsync(99, updateDto);

            // Assert
            result.Should().BeFalse();
            // Asegurarse de que el método Update nunca se llamó
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<TipoDocumento>()), Times.Never);
        }

        // -----------------------------------------------------------------
        // --- PRUEBAS DE ELIMINACIÓN (Delete) ---
        // -----------------------------------------------------------------

        [Fact]
        public async Task DeleteAsync_CallsRepoDelete()
        {
            // Arrange
            _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteAsync(1);

            // Assert
            result.Should().BeTrue();
            // Verificar que el método Delete del repositorio fue llamado
            _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}
