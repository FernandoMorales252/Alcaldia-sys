using AlcaldiaApi.DtOs.DocumentoDtos;
using AlcaldiaApi.DTOs.EmpleadoDTOs;
using AlcaldiaApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlcaldiaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController : Controller
    {
        private readonly IEmpleadoService _service; // Inyección de dependencia

        //|Constructor
        public EmpleadoController(IEmpleadoService service){_service = service;  }

        // GET: api/<EmpleadoController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
           => Ok(await _service.GetAllAsync());

        // GET api/<EmpleadoController>/5
        [HttpGet("{Id_empleado:int}")]
        public async Task<IActionResult> GetById(int Id_empleado)
        {
            var item = await _service.GetByIdAsync(Id_empleado);
            return item is null ? NotFound() : Ok(item);
        }

        // POST api/<EmpleadoController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmpleadoCrearDTo dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { Id_empleado = created.Id_empleado }, created);
        }

        // PUT api/<EmpleadoController>/5
        [HttpPut("{Id_empleado:int}")]
        public async Task<IActionResult> Update(int Id_empleado, [FromBody] EmpleadoActualizarDTo dto)
        {
            var ok = await _service.UpdateAsync(Id_empleado, dto);
            return ok ? NoContent() : NotFound();
        }

        // DELETE api/<EmpleadoController>/5
        [HttpDelete("{Id_empleado:int}")]
        public async Task<IActionResult> Delete(int Id_empleado)
        {
            var ok = await _service.DeleteAsync(Id_empleado);
            return ok ? NoContent() : NotFound();
        }
    }
}
