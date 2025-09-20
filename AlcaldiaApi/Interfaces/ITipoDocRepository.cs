using AlcaldiaApi.Entidades;

namespace AlcaldiaApi.Interfaces
{
    public interface ITipoDocRepository
    {
        Task<List<TipoDocumento>> GetAllAsync();
        Task<TipoDocumento?> GetByIdAsync(int Id_tipo);
        Task<TipoDocumento> AddAsync(TipoDocumento entity);
        Task<bool> UpdateAsync(TipoDocumento entity);
        Task<bool> DeleteAsync(int Id_tipo);
    }
}
