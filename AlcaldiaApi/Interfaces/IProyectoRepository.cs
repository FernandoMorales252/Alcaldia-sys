using AlcaldiaApi.Entidades;

namespace AlcaldiaApi.Interfaces
{
    public interface IProyectoRepository
    {
        Task<List<Proyecto>> GetAllAsync();
        Task<Proyecto?> GetByIdAsync(int Id_proyecto);
        Task<Proyecto> AddAsync(Proyecto entity);
        Task<bool> UpdateAsync(Proyecto entity);
        Task<bool> DeleteAsync(int Id_proyecto);
    }
}
