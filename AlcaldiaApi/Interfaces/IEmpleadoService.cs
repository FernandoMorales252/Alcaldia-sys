using AlcaldiaApi.DtOs.DocumentoDtos;
using AlcaldiaApi.DTOs.CargoDTOs;
using AlcaldiaApi.DTOs.EmpleadoDTOs;

namespace AlcaldiaApi.Interfaces
{
    public interface IEmpleadoService
    {
        Task<List<EmpleadoRespuestaDTo>> GetAllAsync();
        Task<EmpleadoRespuestaDTo?> GetByIdAsync(int Id_empleado);
        Task<EmpleadoRespuestaDTo> CreateAsync(EmpleadoCrearDTo dto);
        Task<bool> UpdateAsync(int Id_empleado, EmpleadoActualizarDTo dto);
        Task<bool> DeleteAsync(int Id_empleado);

        Task<byte[]> ExportarEmpleadosAExcelAsync();
    }
}
