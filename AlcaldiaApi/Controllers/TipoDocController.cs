using AlcaldiaApi.DtOs.TipoDocumentoDTOs;
using AlcaldiaApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlcaldiaApi.Controllers
{
    //Anotacion para definir que es un controlador de API
    [ApiController]
    [Route("api/[controller]")]
    public class TipoDocController : ControllerBase
    {
        // 1.Inyeccion de dependencia del servicio
        private readonly ITipoDocService _service;

        // 2.Inyeccion de dependencia del servicio
        public TipoDocController(ITipoDocService service)
        {
            _service = service;
        }

        // 3.Metodo para obtener todos los tipos de documento
        [HttpGet]
        public async Task<IActionResult> GetAll()
           => Ok(await _service.GetAllAsync());

        // 4.Metodo para obtener los tipos de documento por ID
        [HttpGet("{Id_tipo:int}")]
        public async Task<IActionResult> GetById(int Id_tipo)
        {
            var item = await _service.GetByIdAsync(Id_tipo);
            return item is null ? NotFound() : Ok(item);
        }

        // 5.Metodo para crear un tipo de documento
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TipoDocumentoCrearDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { Id_tipo = created.Id_tipo }, created);
        }

        // 6.Metodo para actualizar un tipo de documento
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int Id_tipo, [FromBody] TipoDocumentoActualizarDTO dto)
        {
            var ok = await _service.UpdateAsync(Id_tipo, dto);
            return ok ? NoContent() : NotFound();
        }

        // 7.Metodo para eliminar un tipo de documento
        [HttpDelete("{Id_tipo:int}")]
        public async Task<IActionResult> Delete(int Id_tipo)
        {
            var ok = await _service.DeleteAsync(Id_tipo);
            return ok ? NoContent() : NotFound();
        }
    }
}
