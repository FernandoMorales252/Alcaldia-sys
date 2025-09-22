using AlcaldiaApi.DTOs.EmpleadoDTOs;
using AlcaldiaApi.Entidades;

namespace AlcaldiaApi.Interfaces
{
    public interface IEmpleadoRepository
    {
        Task<List<Empleado>> GetAllAsync();
        Task<Empleado?> GetByIdAsync(int Id_empleado);
        Task<Empleado> AddAsync(Empleado entity);
        Task<bool> UpdateAsync(Empleado entity);
        Task<bool> DeleteAsync(int Id_empleado);
    }
}
