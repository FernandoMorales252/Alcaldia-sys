using AlcaldiaApi.DTOs.MunicipioDTOs;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;

namespace AlcaldiaApi.Servicios
{
    public class MunicipioService : IMunicipioService
    {
        private readonly IMunicipioRepository _repo;
        public MunicipioService(IMunicipioRepository repo) => _repo = repo;
        public async Task<List<MunicipioRespuestaDTo>> GetAllAsync() =>
            (await _repo.GetAllAsync()).Select(x => new MunicipioRespuestaDTo
            {
                Id_Municipio = x.Id_Municipio,
                Nombre_Municipio = x.Nombre_Municipio,
            }).ToList();
        public async Task<MunicipioRespuestaDTo?> GetByIdAsync(int id)
        {
            var x = await _repo.GetByIdAsync(id);
            return x == null ? null : new MunicipioRespuestaDTo
            {
                Id_Municipio = x.Id_Municipio,
                Nombre_Municipio = x.Nombre_Municipio,
            };
        }
        public async Task<MunicipioRespuestaDTo> CreateAsync(MunicipioCrearDTo dto)
        {
            var entity = new Municipio { Nombre_Municipio = dto.Nombre_Municipio.Trim() };
            var saved = await _repo.AddAsync(entity);
            return new MunicipioRespuestaDTo { Id_Municipio = saved.Id_Municipio, Nombre_Municipio = saved.Nombre_Municipio };
        }

        public async Task<bool> UpdateAsync(int Id_Municipio, MunicipioActualizarDTo dto)
        {
            var current = await _repo.GetByIdAsync(Id_Municipio);
            if (current == null) return false;
            current.Nombre_Municipio = dto.Nombre_Municipio.Trim();
            return await _repo.UpdateAsync(current);
        }

        public Task<bool> DeleteAsync(int Id_Municipio) => _repo.DeleteAsync(Id_Municipio);
    }
}
