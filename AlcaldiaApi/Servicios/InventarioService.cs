using AlcaldiaApi.DtOs.InventarioDTOs;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;

namespace AlcaldiaApi.Servicios
{
    public class InventarioService : IInventarioService
    {
        private readonly IInventarioRepository _repo;

        public InventarioService(IInventarioRepository repo) => _repo = repo;

        // 1.Servicio para obtener todos los documentos
        public async Task<List<InventarioRespuestaDTO>> GetAllAsync() =>
            (await _repo.GetAllAsync()).Select(x => new InventarioRespuestaDTO
            {
                Id_inventario = x.Id_inventario,
                Nombre_item = x.Nombre_item,
                Descripcion = x.Descripcion,
                Cantidad = x.Cantidad,
                Fecha_ingreso = x.Fecha_ingreso,
                Estado = x.Estado,
                MunicipioId = x.MunicipioId
            }).ToList();

        // 2.Servicio para obtener documento por ID
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
                MunicipioId = x.MunicipioId
            };
        }


        // 3.Servicio para crear un documento
        public async Task<InventarioRespuestaDTO> CreateAsync(InventarioCrearDTO dto)
        {
            var entity = new Inventario
            {
                Nombre_item = dto.Nombre_item.Trim(),
                Descripcion = dto.Descripcion.Trim(),
                Cantidad = dto.Cantidad,
                Fecha_ingreso = dto.Fecha_ingreso,
                Estado = dto.Estado.Trim(),
                MunicipioId = dto.MunicipioId, // Si el dato que esta en la entidad es int , date o un dato que no sea tipo string 
            };                                                                                                                                 // Entonces no se usa .Trim() y solo se hace por ejemplo 
                                                                                                                                               // TipoDocumentoId = dto.TipoDocumentoId , al ser int solo se pone = dto.nombredelcampo
            var saved = await _repo.AddAsync(entity);
            return new InventarioRespuestaDTO
            {
                Id_inventario = saved.Id_inventario,
                Nombre_item = saved.Nombre_item,
                Descripcion = saved.Descripcion,
                Cantidad = saved.Cantidad,
                Fecha_ingreso = saved.Fecha_ingreso,
                Estado = saved.Estado,
                MunicipioId = saved.MunicipioId,//var es para que Guarden todos los datos incluyendo el id
            };
        }

        // 4.Servicio para actualizar un documento
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
            return await _repo.UpdateAsync(current);
        }

        // 5.Servicio para eliminar un documento por ID
        public Task<bool> DeleteAsync(int Id_inventario) => _repo.DeleteAsync(Id_inventario);
    }
}
