using AlcaldiaApi.DtOs.DocumentoDtos;
using AlcaldiaApi.DtOs.InventarioDTOs;
using AlcaldiaApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlcaldiaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventarioController : ControllerBase
    {
        private readonly IInventarioService _service;

        // 2.Inyeccion de dependencia del servicio
        public InventarioController(IInventarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
           => Ok(await _service.GetAllAsync());

        [HttpGet("{Id_inventario:int}")]
        public async Task<IActionResult> GetById(int Id_inventario)
        {
            var item = await _service.GetByIdAsync(Id_inventario);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InventarioCrearDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { Id_inventario = created.Id_inventario }, created);
        }

        [HttpPut("{Id_inventario}")]
        public async Task<IActionResult> Update(int Id_inventario, [FromBody] InventarioActualizarDTo dto)
        {
            var ok = await _service.UpdateAsync(Id_inventario, dto);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{Id_inventario:int}")]
        public async Task<IActionResult> Delete(int Id_inventario)
        {
            var ok = await _service.DeleteAsync(Id_inventario);
            return ok ? NoContent() : NotFound();
        }
    }
}
