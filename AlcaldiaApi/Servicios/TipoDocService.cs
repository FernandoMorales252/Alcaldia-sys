using AlcaldiaApi.DtOs.TipoDocumentoDTOs;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;

namespace AlcaldiaApi.Servicios
{
    public class TipoDocService : ITipoDocService
    {
        private readonly ITipoDocRepository _repo;

        public TipoDocService(ITipoDocRepository repo) => _repo = repo;

        public async Task<List<TipoDocumentoRespuestaDTO>> GetAllAsync() =>
            (await _repo.GetAllAsync()).Select(x => new TipoDocumentoRespuestaDTO
            {
                Id_tipo = x.Id_tipo,
                Nombre = x.Nombre,
            }).ToList();

        public async Task<TipoDocumentoRespuestaDTO?> GetByIdAsync(int id)
        {
            var x = await _repo.GetByIdAsync(id);
            return x == null ? null : new TipoDocumentoRespuestaDTO
            {
                Id_tipo = x.Id_tipo,
                Nombre = x.Nombre,
            };
        }

        public async Task<TipoDocumentoRespuestaDTO> CreateAsync(TipoDocumentoCrearDTO dto)
        {
            var entity = new TipoDocumento { Nombre = dto.Nombre.Trim() };
            var saved = await _repo.AddAsync(entity);
            return new TipoDocumentoRespuestaDTO { Id_tipo = saved.Id_tipo, Nombre = saved.Nombre };
        }

        public async Task<bool> UpdateAsync(int Id_tipo, TipoDocumentoActualizarDTO dto)
        {
            var current = await _repo.GetByIdAsync(Id_tipo);
            if (current == null) return false;
            current.Nombre = dto.Nombre.Trim();
            return await _repo.UpdateAsync(current);
        }

        public Task<bool> DeleteAsync(int Id_tipo) => _repo.DeleteAsync(Id_tipo);
    }
}
