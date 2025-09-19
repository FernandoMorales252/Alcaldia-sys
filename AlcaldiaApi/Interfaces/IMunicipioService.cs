using AlcaldiaApi.DTOs.MunicipioDTOs;

namespace AlcaldiaApi.Interfaces
{
    public interface IMunicipioService
    {
        Task<List<MunicipioRespuestaDTo>> GetAllAsync();
        Task<MunicipioRespuestaDTo?> GetByIdAsync(int Id_municipio);
        Task<MunicipioRespuestaDTo> CreateAsync(MunicipioCrearDTo dto);
        Task<bool> UpdateAsync(int Id_municipio, MunicipioActualizarDTo dto);
        Task<bool> DeleteAsync(int Id_municipio);
    }
}
