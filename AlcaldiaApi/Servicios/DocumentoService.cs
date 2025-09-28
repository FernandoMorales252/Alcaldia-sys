using AlcaldiaApi.DtOs.DocumentoDtos;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;

namespace AlcaldiaApi.Servicios
{
    public class DocumentoService : IDocumentoService
    {
        private readonly IDocumentoRepository _repo;

        public DocumentoService(IDocumentoRepository repo) => _repo = repo;

        // 1.Servicio para obtener todos los documentos
        public async Task<List<DocumentoARespuestaDTO>> GetAllAsync() =>
            (await _repo.GetAllAsync()).Select(x => new DocumentoARespuestaDTO
            {
                Id_documento = x.Id_documento,
                Numero_documento = x.Numero_documento,
                Fecha_emision = x.Fecha_emision,
                Propietario = x.Propietario,
                Detalles = x.Detalles,
                Estado = x.Estado,
                TipoDocumentoId = x.TipoDocumentoId,
                MunicipioId = x.MunicipioId,

            }).ToList();

        // 2.Servicio para obtener documento por ID
        public async Task<DocumentoARespuestaDTO?> GetByIdAsync(int id)
        {
            var x = await _repo.GetByIdAsync(id);
            return x == null ? null : new DocumentoARespuestaDTO
            {
                Id_documento = x.Id_documento,
                Numero_documento = x.Numero_documento,
                Fecha_emision = x.Fecha_emision,
                Propietario = x.Propietario,
                Detalles = x.Detalles,
                Estado = x.Estado,
                TipoDocumentoId = x.TipoDocumentoId,
                MunicipioId = x.MunicipioId,
            };
        }


        // 3.Servicio para crear un documento
        public async Task<DocumentoARespuestaDTO> CreateAsync(DocumentoCrearDTO dto)
        {
            var entity = new Documento { Numero_documento = dto.Numero_documento.Trim(), Fecha_emision = dto.Fecha_emision,Propietario = dto.Propietario.Trim(),
                Detalles = dto.Detalles.Trim(),Estado = dto.Estado.Trim(),TipoDocumentoId = dto.TipoDocumentoId,MunicipioId = dto.MunicipioId, // Si el dato que esta en la entidad es int , date o un dato que no sea tipo string 
            };                                                                                                                                 // Entonces no se usa .Trim() y solo se hace por ejemplo 
                                                                                                                                               // TipoDocumentoId = dto.TipoDocumentoId , al ser int solo se pone = dto.nombredelcampo
            var saved = await _repo.AddAsync(entity);
            return new DocumentoARespuestaDTO { Id_documento = saved.Id_documento, Numero_documento = saved.Numero_documento ,Fecha_emision = saved.Fecha_emision,Propietario = saved.Propietario,
                Detalles = saved.Detalles, Estado = saved.Estado,TipoDocumentoId = saved.TipoDocumentoId ,MunicipioId = saved.MunicipioId //var es para que Guarden todos los datos incluyendo el id
            };
        }

        // 4.Servicio para actualizar un documento
        public async Task<bool> UpdateAsync(int Id_documento, DocumentoActualizarDTO dto)
        {
            var current = await _repo.GetByIdAsync(Id_documento);
            if (current == null) return false;
            current.Numero_documento = dto.Numero_documento.Trim();
            current.Propietario = dto.Propietario.Trim();
            current.Detalles = dto.Detalles.Trim();
            current.Estado = dto.Estado.Trim();
            current.Fecha_emision = dto.Fecha_emision; // No se usa .Trim() al ser un dato tipo date
            current.TipoDocumentoId = dto.TipoDocumentoId; // No se usa .Trim() al ser un dato tipo int
            current.MunicipioId = dto.MunicipioId;
            return await _repo.UpdateAsync(current);
        }

        // 5.Servicio para eliminar un documento por ID
        public Task<bool> DeleteAsync(int Id_documento) => _repo.DeleteAsync(Id_documento);
    }
}
