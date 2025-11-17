using AlcaldiaApi.DTOs.EmpleadoDTOs;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;
using OfficeOpenXml;

namespace AlcaldiaApi.Servicios
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly IEmpleadoRepository _repo; //Repositorio

        public EmpleadoService(IEmpleadoRepository repo)
        {
            _repo = repo;
            // Configuración de licencia obligatoria para EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        //Metodo para obtener todos
        public async Task<List<EmpleadoRespuestaDTo>> GetAllAsync() =>
            (await _repo.GetAllAsync()).Select(x => new EmpleadoRespuestaDTo
            {
                Id_empleado = x.Id_empleado,
                Nombre = x.Nombre,
                Apellido = x.Apellido,
                Fecha_contratacion = x.Fecha_contratacion,
                Estado = x.Estado,
                CargoId = x.CargoId,
                MunicipioId = x.MunicipioId,

            }).ToList();

        //Metodo para obtener por Id
        public async Task<EmpleadoRespuestaDTo?> GetByIdAsync(int Id_empleado)
        {
            var x = await _repo.GetByIdAsync(Id_empleado);
            return x == null ? null : new EmpleadoRespuestaDTo
            {
                Id_empleado = x.Id_empleado,
                Nombre = x.Nombre,
                Apellido = x.Apellido,
                Fecha_contratacion = x.Fecha_contratacion,
                Estado = x.Estado,
                CargoId = x.CargoId,
                MunicipioId = x.MunicipioId,
            };
        }


        //Metodo para crear
        public async Task<EmpleadoRespuestaDTo> CreateAsync(EmpleadoCrearDTo dto)
        {
            var entity = new Empleado
            {
                Nombre = dto.Nombre.Trim(),
                Apellido = dto.Apellido.Trim(),
                Fecha_contratacion = dto.Fecha_contratacion,
                Estado = dto.Estado.Trim(),
                CargoId = dto.CargoId,
                MunicipioId = dto.MunicipioId,
            };

            var saved = await _repo.AddAsync(entity);
            return new EmpleadoRespuestaDTo
            {
                Id_empleado = saved.Id_empleado,
                Nombre = saved.Nombre,
                Apellido = saved.Apellido,
                Fecha_contratacion = saved.Fecha_contratacion,
                Estado = saved.Estado,
                CargoId = saved.CargoId,
                MunicipioId = saved.MunicipioId,
            };
        }


        //Metodo para actualizar
        public async Task<bool> UpdateAsync(int Id_empleado, EmpleadoActualizarDTo dto)
        {
            var current = await _repo.GetByIdAsync(Id_empleado);
            if (current == null) return false;
            current.Nombre = dto.Nombre.Trim();
            current.Apellido = dto.Apellido.Trim();
            current.Fecha_contratacion = dto.Fecha_contratacion;
            current.Estado = dto.Estado.Trim();
            current.CargoId = dto.CargoId;
            current.MunicipioId = dto.MunicipioId;

            return await _repo.UpdateAsync(current);
        }

        //Metodo para eliminar
        public Task<bool> DeleteAsync(int Id_empleado) => _repo.DeleteAsync(Id_empleado);


        public async Task<byte[]> ExportarEmpleadosAExcelAsync()
        {
            // 1. Obtener los datos usando el método existente (que ya mapea a DTOs)
            var empleados = await GetAllAsync(); // Obtiene List<EmpleadoRespuestaDTo>

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Listado Empleados");

                // --- Definición de Encabezados (Coincide con EmpleadoRespuestaDTo) ---
                worksheet.Cells[1, 1].Value = "ID Empleado";
                worksheet.Cells[1, 2].Value = "Nombre";
                worksheet.Cells[1, 3].Value = "Apellido";
                worksheet.Cells[1, 4].Value = "Fecha Contratación";
                worksheet.Cells[1, 5].Value = "Estado";
                worksheet.Cells[1, 6].Value = "ID Cargo";
                worksheet.Cells[1, 7].Value = "ID Municipio";

                // Estilo de Encabezado
                using (var range = worksheet.Cells[1, 1, 1, 7]) // 7 columnas
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(52, 152, 219)); // Azul Claro
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                // --- Datos ---
                int row = 2;
                foreach (var empleado in empleados)
                {
                    worksheet.Cells[row, 1].Value = empleado.Id_empleado;
                    worksheet.Cells[row, 2].Value = empleado.Nombre;
                    worksheet.Cells[row, 3].Value = empleado.Apellido;

                    // Formato de fecha para Excel
                    worksheet.Cells[row, 4].Value = empleado.Fecha_contratacion.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 5].Value = empleado.Estado;
                    worksheet.Cells[row, 6].Value = empleado.CargoId;
                    worksheet.Cells[row, 7].Value = empleado.MunicipioId;
                    row++;
                }

                // Aplicar Bordes a toda la tabla
                using (var range = worksheet.Cells[1, 1, row - 1, 7])
                {
                    range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                }

                // Autoajustar columnas
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Guardar el paquete Excel en un MemoryStream y devolver el array de bytes
                var stream = new MemoryStream();
                package.SaveAs(stream);

                return stream.ToArray();
            }

        }
    }
}
