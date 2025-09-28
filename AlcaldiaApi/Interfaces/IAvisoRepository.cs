using AlcaldiaApi.Entidades;

namespace AlcaldiaApi.Interfaces
{
    public interface IAvisoRepository
    {
        Task<List<Aviso>> GetAllAsync();
        Task<Aviso?> GetByIdAsync(int Id_aviso);
        Task<Aviso> AddAsync(Aviso entity);
        Task<bool> UpdateAsync(Aviso entity);
        Task<bool> DeleteAsync(int Id_aviso);
    }
}
