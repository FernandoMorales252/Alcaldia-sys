using AlcaldiaApi.DtOs.DocumentoDtos;
using AlcaldiaApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlcaldiaApi.Controllers
{
    //Anotacion para definir que es un controlador de API
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentoController : ControllerBase
    {
        // 1.Inyeccion de dependencia del servicio
        private readonly IDocumentoService _service;

        // 2.Inyeccion de dependencia del servicio
        public DocumentoController(IDocumentoService service)
        {
            _service = service;
        }

        // 3.Metodo para obtener todos los documentos
        [HttpGet]
        public async Task<IActionResult> GetAll()
           => Ok(await _service.GetAllAsync());

        // 4.Metodo para obtener los documentos por ID
        [HttpGet("{Id_documento:int}")]
        public async Task<IActionResult> GetById(int Id_documento)
        {
            var item = await _service.GetByIdAsync(Id_documento);
            return item is null ? NotFound() : Ok(item);
        }

        // 5.Metodo para crear un documento
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DocumentoCrearDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { Id_documento = created.Id_documento }, created);
        }

        // 6.Metodo para actualizar documento
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int Id_documento, [FromBody] DocumentoActualizarDTO dto)
        {
            var ok = await _service.UpdateAsync(Id_documento, dto);
            return ok ? NoContent() : NotFound();
        }

        // 7.Metodo para eliminar un documento
        [HttpDelete("{Id_documento:int}")]
        public async Task<IActionResult> Delete(int Id_documento)
        {
            var ok = await _service.DeleteAsync(Id_documento);
            return ok ? NoContent() : NotFound();
        }

    }
}
