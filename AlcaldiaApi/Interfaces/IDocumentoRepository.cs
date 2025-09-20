using AlcaldiaApi.Entidades;

namespace AlcaldiaApi.Interfaces
{
    public interface IDocumentoRepository
    {
        Task<List<Documento>> GetAllAsync();
        Task<Documento?> GetByIdAsync(int Id_documento);
        Task<Documento> AddAsync(Documento entity);
        Task<bool> UpdateAsync(Documento entity);
        Task<bool> DeleteAsync(int Id_documento);
    }
}
