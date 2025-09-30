using AlcaldiaApi.DtOs.DocumentoDtos;
using AlcaldiaApi.DTOs.AvisoDTOs;
using AlcaldiaApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlcaldiaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvisoController : ControllerBase
    {
        // 1.Inyeccion de dependencia del servicio
        private readonly IAvisoService _service;

        // 2.Inyeccion de dependencia del servicio
        public AvisoController(IAvisoService service)
        {
            _service = service;
        }

        // 3.Metodo para obtener todos los documentos
        [HttpGet]
        public async Task<IActionResult> GetAll()
           => Ok(await _service.GetAllAsync());

        // 4.Metodo para obtener los documentos por ID
        [HttpGet("{Id_aviso:int}")]
        public async Task<IActionResult> GetById(int Id_aviso)
        {
            var item = await _service.GetByIdAsync(Id_aviso);
            return item is null ? NotFound() : Ok(item);
        }

        // 5.Metodo para crear un documento
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AvisoCrearDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { Id_aviso = created.Id_aviso }, created);
        }

        // 6.Metodo para actualizar documento
        [HttpPut("{Id_aviso}")]
        public async Task<IActionResult> Update(int Id_aviso, [FromBody] AvisoActualizarDTo dto)
        {
            var ok = await _service.UpdateAsync(Id_aviso, dto);
            return ok ? NoContent() : NotFound();
        }

        // 7.Metodo para eliminar un documento
        [HttpDelete("{Id_aviso:int}")]
        public async Task<IActionResult> Delete(int Id_aviso)
        {
            var ok = await _service.DeleteAsync(Id_aviso);
            return ok ? NoContent() : NotFound();
        }
    }
}
