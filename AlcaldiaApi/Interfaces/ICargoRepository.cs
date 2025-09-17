using AlcaldiaApi.Entidades;

namespace AlcaldiaApi.Interfaces
{
    public interface ICargoRepository
    {
        Task<List<Cargo>> GetAllAsync();
        Task<Cargo?> GetByIdAsync(int Id_Cargo);
        Task<Cargo> AddAsync(Cargo entity);
        Task<bool> UpdateAsync(Cargo entity);
        Task<bool> DeleteAsync(int Id_Cargo);
    }
}
