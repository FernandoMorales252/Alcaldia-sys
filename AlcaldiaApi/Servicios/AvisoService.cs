using AlcaldiaApi.DTOs.AvisoDTOs;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;

namespace AlcaldiaApi.Servicios
{
    public class AvisoService :IAvisoService
    {
        private readonly IAvisoRepository _repo;
        public AvisoService(IAvisoRepository repo) => _repo = repo;
        public async Task<List<AvisoRespuestaDTO>> GetAllAsync() =>
            (await _repo.GetAllAsync()).Select(x => new AvisoRespuestaDTO
            {
                Id_aviso = x.Id_aviso,
                Titulo = x.Titulo,
                Descripcion = x.Descripcion,
                Fecha_Registro = x.Fecha_Registro,
                Tipo = x.Tipo,
                MunicipioId = x.MunicipioId
            }).ToList();
        public async Task<AvisoRespuestaDTO?> GetByIdAsync(int id)
        {
            var x = await _repo.GetByIdAsync(id);
            return x == null ? null : new AvisoRespuestaDTO
            {
                Id_aviso = x.Id_aviso,
                Titulo = x.Titulo,
                Descripcion = x.Descripcion,
                Fecha_Registro = x.Fecha_Registro,
                Tipo = x.Tipo,
                MunicipioId = x.MunicipioId
            };
        }
        public async Task<AvisoRespuestaDTO> CreateAsync(AvisoCrearDTO dto)
        {
            var entity = new Aviso { Titulo = dto.Titulo.Trim(), Descripcion = dto.Descripcion.Trim(), Fecha_Registro = dto.Fecha_Registro, Tipo = dto.Tipo, MunicipioId = dto.MunicipioId };
            var saved = await _repo.AddAsync(entity);
            return new AvisoRespuestaDTO { Id_aviso = saved.Id_aviso, Titulo = saved.Titulo, Descripcion = saved.Descripcion, Fecha_Registro = saved.Fecha_Registro, Tipo = saved.Tipo, MunicipioId = saved.MunicipioId };
        }

        public async Task<bool> UpdateAsync(int Id_aviso, AvisoActualizarDTo dto)
        {
            var current = await _repo.GetByIdAsync(Id_aviso);
            if (current == null) return false;
            current.Titulo = dto.Titulo.Trim();
            current.Descripcion = dto.Descripcion.Trim();
            current.Fecha_Registro= dto.Fecha_Registro;
            current.Tipo = dto.Tipo.Trim();
            current.MunicipioId = dto.MunicipioId;
            return await _repo.UpdateAsync(current);
        }

        public Task<bool> DeleteAsync(int Id_aviso) => _repo.DeleteAsync(Id_aviso);
    }
}

