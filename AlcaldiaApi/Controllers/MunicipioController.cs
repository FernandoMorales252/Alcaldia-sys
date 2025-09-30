using AlcaldiaApi.DTOs.MunicipioDTOs;
using AlcaldiaApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlcaldiaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MunicipioController : ControllerBase
    {
        private readonly IMunicipioService _service;
        public MunicipioController(IMunicipioService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
           => Ok(await _service.GetAllAsync());

        [HttpGet("{Id_Municipio:int}")]
        public async Task<IActionResult> GetById(int Id_Municipio)
        {
            var item = await _service.GetByIdAsync(Id_Municipio);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MunicipioCrearDTo dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { Id_Municipio = created.Id_Municipio }, created);
        }

        [HttpPut("{Id_Municipio}")]
        public async Task<IActionResult> Update(int Id_Municipio, [FromBody] MunicipioActualizarDTo dto)
        {
            var ok = await _service.UpdateAsync(Id_Municipio, dto);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{Id_Municipio:int}")]
        public async Task<IActionResult> Delete(int Id_Municipio)
        {
            var ok = await _service.DeleteAsync(Id_Municipio);
            return ok ? NoContent() : NotFound();
        }
    }
}
