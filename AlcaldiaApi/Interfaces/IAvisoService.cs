using AlcaldiaApi.DTOs.AvisoDTOs;

namespace AlcaldiaApi.Interfaces
{
    public interface IAvisoService
    {
        Task<List<AvisoRespuestaDTO>> GetAllAsync();
        Task<AvisoRespuestaDTO?> GetByIdAsync(int Id_aviso);
        Task<AvisoRespuestaDTO> CreateAsync(AvisoCrearDTO dto);
        Task<bool> UpdateAsync(int Id_aviso, AvisoActualizarDTo dto);
        Task<bool> DeleteAsync(int Id_aviso);
    }
}
