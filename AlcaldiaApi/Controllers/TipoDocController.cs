using AlcaldiaApi.DtOs.TipoDocumentoDTOs;
using AlcaldiaApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlcaldiaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoDocController : ControllerBase
    {
        private readonly ITipoDocService _service;
        public TipoDocController(ITipoDocService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
           => Ok(await _service.GetAllAsync());


        [HttpGet("{Id_tipo:int}")]
        public async Task<IActionResult> GetById(int Id_tipo)
        {
            var item = await _service.GetByIdAsync(Id_tipo);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TipoDocumentoCrearDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { Id_tipo = created.Id_tipo }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int Id_tipo, [FromBody] TipoDocumentoActualizarDTO dto)
        {
            var ok = await _service.UpdateAsync(Id_tipo, dto);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{Id_tipo:int}")]
        public async Task<IActionResult> Delete(int Id_tipo)
        {
            var ok = await _service.DeleteAsync(Id_tipo);
            return ok ? NoContent() : NotFound();
        }
    }
}
