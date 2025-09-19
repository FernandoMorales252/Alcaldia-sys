using AlcaldiaApi.Entidades;

namespace AlcaldiaApi.Interfaces
{
    public interface IMunicipioRepository
    {
        Task<List<Municipio>> GetAllAsync();
        Task<Municipio?> GetByIdAsync(int Id_Municipio);
        Task<Municipio> AddAsync(Municipio entity);
        Task<bool> UpdateAsync(Municipio entity);
        Task<bool> DeleteAsync(int Id_Municipio);
    }
}

