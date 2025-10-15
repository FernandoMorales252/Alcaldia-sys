using Xunit;
using Moq;
using FluentAssertions;
using AlcaldiaApi.Interfaces;
using AlcaldiaApi.Servicios;
using AlcaldiaApi.DTOs.MunicipioDTOs; 
using AlcaldiaApi.Entidades;

namespace AlcaldiaApi.Tests.Unit.Services
{
    
    public class MunicipioServiceTests
    {
        
        private readonly Mock<IMunicipioRepository> _mockRepo = new();
        private readonly MunicipioService _sut;

        
        public MunicipioServiceTests()
        {
            _sut = new MunicipioService(_mockRepo.Object);
        }

        // Método auxiliar para crear una entidad Municipio de prueba
        private Municipio GetTestEntity(int id = 1, string nombre = "San Salvador") =>
            new Municipio { Id_Municipio = id, Nombre_Municipio = nombre };


        // -----------------------------------------------------------------
        // --- PRUEBAS DE LECTURA (Read) ---
        // -----------------------------------------------------------------

        [Fact]
        public async Task GetAllAsync_ReturnsList_WhenItemsExist()
        {
            // Arrange
            var entities = new List<Municipio> {
                GetTestEntity(1, "San Salvador"),
                GetTestEntity(2, "Santa Ana")
            };
            
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Should().BeOfType<List<MunicipioRespuestaDTo>>();
            result.Should().HaveCount(2);
            result.First().Nombre_Municipio.Should().Be("San Salvador");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsDTO_WhenFound()
        {
            // Arrange
            var entity = GetTestEntity(10, "Apopa");
            
            _mockRepo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(entity);

            // Act
            var result = await _sut.GetByIdAsync(10);

            // Assert
            result.Should().NotBeNull();
            result!.Id_Municipio.Should().Be(10);
            result.Nombre_Municipio.Should().Be("Apopa");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Municipio)null);

            // Act
            var result = await _sut.GetByIdAsync(99);

            // Assert
            result.Should().BeNull();
        }

        // -----------------------------------------------------------------
        // --- PRUEBAS DE CREACIÓN (Create) ---
        // -----------------------------------------------------------------

        [Fact]
        public async Task CreateAsync_CallsRepoAddAndReturnsDTO_WithTrimmedName()
        {
            // Arrange
            var createDto = new MunicipioCrearDTo { Nombre_Municipio = "  Mejicanos  " };
            
            var savedEntity = GetTestEntity(id: 3, nombre: "Mejicanos");

            
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Municipio>())).ReturnsAsync(savedEntity);

            // Act
            var result = await _sut.CreateAsync(createDto);

            // Assert
            result.Should().BeOfType<MunicipioRespuestaDTo>();
            result.Id_Municipio.Should().Be(3);
            result.Nombre_Municipio.Should().Be("Mejicanos"); 

            
            _mockRepo.Verify(
                r => r.AddAsync(It.Is<Municipio>(e => e.Nombre_Municipio == "Mejicanos")),
                Times.Once);
        }

        // -----------------------------------------------------------------
        // --- PRUEBAS DE ACTUALIZACIÓN (Update) ---
        // -----------------------------------------------------------------

        [Fact]
        public async Task UpdateAsync_ReturnsTrue_WhenSuccessful()
        {
            // Arrange
            var updateDto = new MunicipioActualizarDTo { Nombre_Municipio = "Soyapango Actualizado" };
            var currentEntity = GetTestEntity(id: 5, nombre: "Soyapango Viejo");

            
            _mockRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(currentEntity);
            
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Municipio>())).ReturnsAsync(true);

            // Act
            var result = await _sut.UpdateAsync(5, updateDto);

            // Assert
            result.Should().BeTrue();

            
            _mockRepo.Verify(r => r.UpdateAsync(
                It.Is<Municipio>(e => e.Nombre_Municipio == "Soyapango Actualizado" && e.Id_Municipio == 5)),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenNotFound()
        {
            // Arrange
            var updateDto = new MunicipioActualizarDTo { Nombre_Municipio = "No existe" };
            
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Municipio)null);

            // Act
            var result = await _sut.UpdateAsync(99, updateDto);

            // Assert
            result.Should().BeFalse();
            
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Municipio>()), Times.Never);
        }

        // -----------------------------------------------------------------
        // --- PRUEBAS DE ELIMINACIÓN (Delete) ---
        // -----------------------------------------------------------------

        [Fact]
        public async Task DeleteAsync_CallsRepoDelete_AndReturnsResult()
        {
            // Arrange
            
            _mockRepo.Setup(r => r.DeleteAsync(7)).ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteAsync(7);

            // Assert
            result.Should().BeTrue();
            
            _mockRepo.Verify(r => r.DeleteAsync(7), Times.Once);
        }
    }
}