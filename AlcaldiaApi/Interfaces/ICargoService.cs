using AlcaldiaApi.DTOs.CargoDTOs;

namespace AlcaldiaApi.Interfaces
{
    public interface ICargoService
    {
        Task<List<CargoRespuestaDTo>> GetAllAsync();
        Task<CargoRespuestaDTo?> GetByIdAsync(int Id_Cargo);
        Task<CargoRespuestaDTo> CreateAsync(CargoCrearDTo dto);
        Task<bool> UpdateAsync(int Id_Cargo, CargoActualizarDTo dto);
        Task<bool> DeleteAsync(int Id_Cargo);
    }
}
