using AlcaldiaApi.DtOs.InventarioDTOs;
using AlcaldiaApi.DTOs.EmpleadoDTOs;

namespace AlcaldiaApi.Interfaces
{
    public interface IInventarioService
    {
        Task<List<InventarioRespuestaDTO>> GetAllAsync();
        Task<InventarioRespuestaDTO?> GetByIdAsync(int Id_inventario);
        Task<InventarioRespuestaDTO> CreateAsync(InventarioCrearDTO dto);
        Task<bool> UpdateAsync(int Id_inventario, InventarioActualizarDTo dto);
        Task<bool> DeleteAsync(int Id_inventario);
    }
}
