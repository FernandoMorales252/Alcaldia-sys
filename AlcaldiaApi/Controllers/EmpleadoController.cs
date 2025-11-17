using AlcaldiaApi.DtOs.DocumentoDtos;
using AlcaldiaApi.DTOs.EmpleadoDTOs;
using AlcaldiaApi.Interfaces;
using AlcaldiaApi.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace AlcaldiaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController : Controller
    {
        private readonly IEmpleadoService _service; // Inyección de dependencia

        //|Constructor
        public EmpleadoController(IEmpleadoService service) { _service = service; }

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
        [HttpPut("{Id_empleado}")]
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

        [HttpGet("ExportarExcel")]
        public async Task<IActionResult> ExportarExcel()
        {
            try
            {
                // 1. Llama al servicio para generar el archivo binario
                var fileBytes = await _service.ExportarEmpleadosAExcelAsync();

                if (fileBytes == null || fileBytes.Length == 0)
                {
                    // Devolver 204 No Content si no hay empleados o falla la generación.
                    return NoContent();
                }

                // 2. Definir el nombre del archivo
                string excelName = $"Empleados_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                // 3. Devolver el array de bytes como un archivo descargable
                return File(
                    fileContents: fileBytes,
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: excelName
                );
            }
            catch (Exception ex)
            {
                // Manejo de errores centralizado (ej: loguear la excepción)
                return StatusCode(500, $"Error interno del servidor al exportar el archivo: {ex.Message}");
            }
        }
    }
}
