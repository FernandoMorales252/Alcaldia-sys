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

        // GET: api/Inventario
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        // GET: api/Inventario/5
        [HttpGet("{Id_inventario:int}")]
        public async Task<IActionResult> GetById(int Id_inventario)
        {
            var item = await _service.GetByIdAsync(Id_inventario);
            return item is null ? NotFound() : Ok(item);
        }

        // POST: api/Inventario
        // Recibe el DTO con ImagenBase64 (string)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InventarioCrearDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            // Retorna 201 Created y la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetById), new { Id_inventario = created.Id_inventario }, created);
        }

        // PUT: api/Inventario/5
        // Recibe el DTO con ImagenBase64 (string)
        [HttpPut("{Id_inventario:int}")] // Aseguramos el tipo int aquí también
        public async Task<IActionResult> Update(int Id_inventario, [FromBody] InventarioActualizarDTo dto)
        {
            var ok = await _service.UpdateAsync(Id_inventario, dto);
            // Retorna 204 No Content si fue exitoso
            return ok ? NoContent() : NotFound();
        }

        // DELETE: api/Inventario/5
        [HttpDelete("{Id_inventario:int}")]
        public async Task<IActionResult> Delete(int Id_inventario)
        {
            var ok = await _service.DeleteAsync(Id_inventario);
            return ok ? NoContent() : NotFound();
        }
    }
}
