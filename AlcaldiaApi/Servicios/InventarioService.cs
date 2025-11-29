using AlcaldiaApi.DtOs.InventarioDTOs;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;

namespace AlcaldiaApi.Servicios
{
    public class InventarioService : IInventarioService
    {
        private readonly IInventarioRepository _repo;

        public InventarioService(IInventarioRepository repo) => _repo = repo;

        // Método privado para manejar la conversión de Base64 a byte[]
        private byte[]? ConvertBase64ToBytes(string? base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String))
            {
                return null; // Retorna null si la cadena es nula o vacía
            }

            // Limpia la cadena de Base64 de metadatos (por ejemplo, "data:image/jpeg;base64,")
            string base64Cleaned = base64String;
            int commaIndex = base64String.IndexOf(',');
            if (commaIndex > 0)
            {
                base64Cleaned = base64String.Substring(commaIndex + 1);
            }

            try
            {
                // La conversión real
                return Convert.FromBase64String(base64Cleaned);
            }
            catch (FormatException)
            {
                // En un servicio real, aquí podrías loguear el error o lanzar una excepción de negocio.
                // Por simplicidad, retornaremos null o un error específico si falla la conversión.
                return null;
            }
        }

        public async Task<InventarioRespuestaDTO?> GetByIdAsync(int id)
        {
            var x = await _repo.GetByIdAsync(id);
            return x == null ? null : new InventarioRespuestaDTO
            {
                Id_inventario = x.Id_inventario,
                Nombre_item = x.Nombre_item,
                Descripcion = x.Descripcion,
                Cantidad = x.Cantidad,
                Fecha_ingreso = x.Fecha_ingreso,
                Estado = x.Estado,
                MunicipioId = x.MunicipioId,
                // **AJUSTE: Incluir la imagen en el DTO de respuesta**
                ImagenData = x.ImagenData
            };
        }

        public async Task<List<InventarioRespuestaDTO>> GetAllAsync() =>
              (await _repo.GetAllAsync()).Select(x => new InventarioRespuestaDTO
              {
                  Id_inventario = x.Id_inventario,
                  Nombre_item = x.Nombre_item,
                  Descripcion = x.Descripcion,
                  Cantidad = x.Cantidad,
                  Fecha_ingreso = x.Fecha_ingreso,
                  Estado = x.Estado,
                  MunicipioId = x.MunicipioId,
                  // **AJUSTE: Incluir la imagen en el DTO de respuesta**
                  ImagenData = x.ImagenData
              }).ToList();


        public async Task<InventarioRespuestaDTO> CreateAsync(InventarioCrearDTO dto)
        {
            var entity = new Inventario
            {
                Nombre_item = dto.Nombre_item.Trim(),
                Descripcion = dto.Descripcion.Trim(),
                Cantidad = dto.Cantidad,
                Fecha_ingreso = dto.Fecha_ingreso,
                Estado = dto.Estado.Trim(),
                MunicipioId = dto.MunicipioId,
                // **AJUSTE CLAVE: Conversión de Base64 a byte[] para la Entidad**
                ImagenData = ConvertBase64ToBytes(dto.ImagenBase64)
            };

            var saved = await _repo.AddAsync(entity);

            return new InventarioRespuestaDTO
            {
                Id_inventario = saved.Id_inventario,
                Nombre_item = saved.Nombre_item,
                Descripcion = saved.Descripcion,
                Cantidad = saved.Cantidad,
                Fecha_ingreso = saved.Fecha_ingreso,
                Estado = saved.Estado,
                MunicipioId = saved.MunicipioId,
                // Incluir la imagen guardada en la respuesta
                ImagenData = saved.ImagenData
            };
        }
        public async Task<bool> UpdateAsync(int Id_inventario, InventarioActualizarDTo dto)
        {
            var current = await _repo.GetByIdAsync(Id_inventario);
            if (current == null) return false;

            current.Nombre_item = dto.Nombre_item.Trim();
            current.Descripcion = dto.Descripcion.Trim();
            current.Cantidad = dto.Cantidad;
            current.Estado = dto.Estado.Trim();
            current.Fecha_ingreso = dto.Fecha_ingreso;
            current.MunicipioId = dto.MunicipioId;

            // **AJUSTE CLAVE: Conversión de Base64 a byte[] para la Actualización**
            // Solo actualiza si se envía una nueva imagen.
            if (!string.IsNullOrWhiteSpace(dto.ImagenBase64))
            {
                current.ImagenData = ConvertBase64ToBytes(dto.ImagenBase64);
            }
            // Si el cliente envía ImagenBase64 como null, la imagen existente se mantiene.
            // Si se desea eliminar la imagen, el cliente debe enviar una cadena vacía o nula,
            // y la lógica de arriba se encarga de dejar `current.ImagenData` como null.

            return await _repo.UpdateAsync(current);
        }

        public Task<bool> DeleteAsync(int Id_inventario) => _repo.DeleteAsync(Id_inventario);
    }
}
