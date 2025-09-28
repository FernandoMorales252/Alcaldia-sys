using AlcaldiaApi.DTOs.QuejaDTOs;

namespace AlcaldiaApi.Interfaces
{
    public interface IQuejaService
    {
        Task<List<QuejaRespuestaDTO>> GetAllAsync();
        Task<QuejaRespuestaDTO?> GetByIdAsync(int Id_queja);
        Task<QuejaRespuestaDTO> CreateAsync(QuejaCrearDTo dto);
        Task<bool> UpdateAsync(int Id_queja, QuejaActualizarDTo dto);
        Task<bool> DeleteAsync(int Id_queja);
    }
}
