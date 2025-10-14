using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using AlcaldiaApi.Interfaces;
using AlcaldiaApi.Controllers;
using AlcaldiaApi.DtOs.TipoDocumentoDTOs;

namespace AlcaldiaApi.Tests.Unit.Controllers
{
    public class TipoDocControllerTests
    {
        private readonly Mock<ITipoDocService> _mockService = new();
        private readonly TipoDocController _sut; // System Under Test

        // --- ARRANGE GLOBAL ---
        public TipoDocControllerTests()
        {
            _sut = new TipoDocController(_mockService.Object);
        }

        private TipoDocumentoRespuestaDTO GetTestDto(int id = 1, string nombre = "DNI") =>
            new TipoDocumentoRespuestaDTO { Id_tipo = id, Nombre = nombre };


        // -----------------------------------------------------------------
        // --- PRUEBAS DE LECTURA (Read) ---
        // -----------------------------------------------------------------

        [Fact]
        public async Task GetById_ReturnsOk_WhenItemFound()
        {
            // Arrange
            var expectedDto = GetTestDto();
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(expectedDto);

            // Act
            var result = await _sut.GetById(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenItemMissing()
        {
            // Arrange
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((TipoDocumentoRespuestaDTO)null);

            // Act
            var result = await _sut.GetById(99);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        // -----------------------------------------------------------------
        // --- PRUEBAS DE CREACIÓN (Create) ---
        // -----------------------------------------------------------------

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_OnSuccess()
        {
            // Arrange
            var createDto = new TipoDocumentoCrearDTO { Nombre = "Pasaporte" };
            var createdDto = GetTestDto(id: 5, nombre: "Pasaporte");
            _mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(createdDto);

            // Act
            var result = await _sut.Create(createDto);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result as CreatedAtActionResult;

            // Verifica que se pasó el Id y el objeto creado en la respuesta 201
            createdResult.RouteValues!["id_tipo"].Should().Be(5);
            createdResult.Value.Should().BeEquivalentTo(createdDto);
        }

        // -----------------------------------------------------------------
        // --- PRUEBAS DE ACTUALIZACIÓN (Update) ---
        // -----------------------------------------------------------------

        [Fact]
        public async Task Update_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var updateDto = new TipoDocumentoActualizarDTO { Nombre = "Actualizado" };
            _mockService.Setup(s => s.UpdateAsync(1, updateDto)).ReturnsAsync(true); // Simular éxito

            // Act
            var result = await _sut.Update(1, updateDto);

            // Assert
            result.Should().BeOfType<NoContentResult>(); // Código 204
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenFailed()
        {
            // Arrange
            var updateDto = new TipoDocumentoActualizarDTO { Nombre = "Fallo" };
            _mockService.Setup(s => s.UpdateAsync(99, updateDto)).ReturnsAsync(false); // Simular fallo (no encontrado)

            // Act
            var result = await _sut.Update(99, updateDto);

            // Assert
            result.Should().BeOfType<NotFoundResult>(); // Código 404
        }

        // -----------------------------------------------------------------
        // --- PRUEBAS DE ELIMINACIÓN (Delete) ---
        // -----------------------------------------------------------------

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _sut.Delete(1);

            // Assert
            result.Should().BeOfType<NoContentResult>(); // Código 204
        }
    }
}