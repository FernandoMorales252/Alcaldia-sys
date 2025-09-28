using AlcaldiaApi.DTOs.ProyectoDTOs;

namespace AlcaldiaApi.Interfaces
{
    public interface IProyectoService
    {
        Task<List<ProyectoRespuestaDTo>> GetAllAsync();
        Task<ProyectoRespuestaDTo?> GetByIdAsync(int Id_Proyecto);
        Task<ProyectoRespuestaDTo> CreateAsync(ProyectoCrearDTo dto);
        Task<bool> UpdateAsync(int Id_proyecto, ProyectoActualizarDTo dto);
        Task<bool> DeleteAsync(int Id_proyecto);
    }
}
