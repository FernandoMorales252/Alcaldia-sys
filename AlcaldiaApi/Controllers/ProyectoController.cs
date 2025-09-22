using AlcaldiaApi.DTOs.ProyectoDTOs;
using AlcaldiaApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlcaldiaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProyectoController : ControllerBase
    {
        private readonly IProyectoService _service;
        public ProyectoController(IProyectoService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
           => Ok(await _service.GetAllAsync());

        [HttpGet("{Id_Proyecto:int}")]
        public async Task<IActionResult> GetById(int Id_Proyecto)
        {
            var item = await _service.GetByIdAsync(Id_Proyecto);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProyectoCrearDTo dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { Id_Proyecto = created.Id_Proyecto }, created);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int Id_Proyecto, [FromBody] ProyectoActualizarDTo dto)
        {
            var ok = await _service.UpdateAsync(Id_Proyecto, dto);
            return ok ? NoContent() : NotFound();
        }
        [HttpDelete("{Id_Proyecto:int}")]
        public async Task<IActionResult> Delete(int Id_Proyecto)
        {
            var ok = await _service.DeleteAsync(Id_Proyecto);
            return ok ? NoContent() : NotFound();
        }
    }
}
