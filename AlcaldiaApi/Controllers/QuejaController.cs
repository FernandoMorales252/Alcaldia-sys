using AlcaldiaApi.DTOs.AvisoDTOs;
using AlcaldiaApi.DTOs.QuejaDTOs;
using AlcaldiaApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlcaldiaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuejaController : ControllerBase
    {
        // 1.Inyeccion de dependencia del servicio
        private readonly IQuejaService _service;

        // 2.Inyeccion de dependencia del servicio
        public QuejaController(IQuejaService service)
        {
            _service = service;
        }

        // 3.Metodo para obtener todos los documentos
        [HttpGet]
        public async Task<IActionResult> GetAll()
           => Ok(await _service.GetAllAsync());

        // 4.Metodo para obtener los documentos por ID
        [HttpGet("{Id_queja:int}")]
        public async Task<IActionResult> GetById(int Id_queja)
        {
            var item = await _service.GetByIdAsync(Id_queja);
            return item is null ? NotFound() : Ok(item);
        }

        // 5.Metodo para crear un documento
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QuejaCrearDTo dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { Id_queja = created.Id_queja }, created);
        }

        // 6.Metodo para actualizar documento
        [HttpPut("{Id_queja}")]
        public async Task<IActionResult> Update(int Id_queja, [FromBody] QuejaActualizarDTo dto)
        {
            var ok = await _service.UpdateAsync(Id_queja, dto);
            return ok ? NoContent() : NotFound();
        }

        // 7.Metodo para eliminar un documento
        [HttpDelete("{Id_queja:int}")]
        public async Task<IActionResult> Delete(int Id_queja)
        {
            var ok = await _service.DeleteAsync(Id_queja);
            return ok ? NoContent() : NotFound();
        }
    }
}
