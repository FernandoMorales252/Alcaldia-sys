using AlcaldiaApi.DTOs.CargoDTOs;
using AlcaldiaApi.Interfaces;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlcaldiaApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]//Usuarios autenticados
    public class CargoController : ControllerBase
    {
        private readonly ICargoService _service;
        public CargoController(ICargoService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
           => Ok(await _service.GetAllAsync());
        [HttpGet("{Id_Cargo:int}")]
        public async Task<IActionResult> GetById(int Id_Cargo)
        {
            var item = await _service.GetByIdAsync(Id_Cargo);
            return item is null ? NotFound() : Ok(item);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CargoCrearDTo dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { Id_Cargo = created.Id_Cargo }, created);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int Id_Cargo, [FromBody] CargoActualizarDTo dto)
        {
            var ok = await _service.UpdateAsync(Id_Cargo, dto);
            return ok ? NoContent() : NotFound();
        }
        [HttpDelete("{Id_Cargo:int}")]
        public async Task<IActionResult> Delete(int Id_Cargo)
        {
            var ok = await _service.DeleteAsync(Id_Cargo);
            return ok ? NoContent() : NotFound();
        }
    }
}
