using AlcaldiaApi.Entidades;

namespace AlcaldiaApi.Interfaces
{
    public interface IInventarioRepository
    {
        Task<List<Inventario>> GetAllAsync();
        Task<Inventario?> GetByIdAsync(int Id_inventario);
        Task<Inventario> AddAsync(Inventario entity);
        Task<bool> UpdateAsync(Inventario entity);
        Task<bool> DeleteAsync(int Id_inventario);
    }
}
