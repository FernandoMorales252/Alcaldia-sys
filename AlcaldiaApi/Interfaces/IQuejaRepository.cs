using AlcaldiaApi.Entidades;

namespace AlcaldiaApi.Interfaces
{
    public interface IQuejaRepository
    {
        Task<List<Queja>> GetAllAsync();
        Task<Queja?> GetByIdAsync(int Id_queja);
        Task<Queja> AddAsync(Queja entity);
        Task<bool> UpdateAsync(Queja entity);
        Task<bool> DeleteAsync(int Id_queja);
    }
}
