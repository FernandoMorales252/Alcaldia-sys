using AlcaldiaApi.DTOs.AvisoDTOs;
using AlcaldiaApi.DTOs.QuejaDTOs;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;

namespace AlcaldiaApi.Servicios
{
    public class QuejaService :IQuejaService
    {
        private readonly IQuejaRepository _repo;
        public QuejaService(IQuejaRepository repo) => _repo = repo;
        public async Task<List<QuejaRespuestaDTO>> GetAllAsync() =>
            (await _repo.GetAllAsync()).Select(x => new QuejaRespuestaDTO
            {
                Id_queja = x.Id_queja,
                Titulo = x.Titulo,
                Descripcion = x.Descripcion,
                Fecha_Registro = x.Fecha_Registro,
                Tipo = x.Tipo,
                Nivel = x.Nivel,
                MunicipioId = x.MunicipioId
            }).ToList();
        public async Task<QuejaRespuestaDTO?> GetByIdAsync(int id)
        {
            var x = await _repo.GetByIdAsync(id);
            return x == null ? null : new QuejaRespuestaDTO
            {
                Id_queja = x.Id_queja,
                Titulo = x.Titulo,
                Descripcion = x.Descripcion,
                Fecha_Registro = x.Fecha_Registro,
                Tipo = x.Tipo,
                Nivel = x.Nivel,
                MunicipioId = x.MunicipioId
            };
        }
        public async Task<QuejaRespuestaDTO> CreateAsync(QuejaCrearDTo dto)
        {
            var entity = new Queja { Titulo = dto.Titulo.Trim(), Descripcion = dto.Descripcion.Trim(), Fecha_Registro = dto.Fecha_Registro, Tipo = dto.Tipo, Nivel = dto.Nivel, MunicipioId = dto.MunicipioId };
            var saved = await _repo.AddAsync(entity);
            return new QuejaRespuestaDTO { Id_queja = saved.Id_queja, Titulo = saved.Titulo, Descripcion = saved.Descripcion, Fecha_Registro = saved.Fecha_Registro, Tipo = saved.Tipo, Nivel = saved.Nivel, MunicipioId = saved.MunicipioId };
        }

        public async Task<bool> UpdateAsync(int Id_queja, QuejaActualizarDTo dto)
        {
            var current = await _repo.GetByIdAsync(Id_queja);
            if (current == null) return false;
            current.Titulo = dto.Titulo.Trim();
            current.Descripcion = dto.Descripcion.Trim();
            current.Fecha_Registro = dto.Fecha_Registro;
            current.Tipo = dto.Tipo.Trim();
            current.Tipo = dto.Tipo.Trim();
            current.MunicipioId = dto.MunicipioId;
            return await _repo.UpdateAsync(current);
        }

        public Task<bool> DeleteAsync(int Id_queja) => _repo.DeleteAsync(Id_queja);
    }
}

