using AlcaldiaApi.DtOs.TipoDocumentoDTOs;

namespace AlcaldiaApi.Interfaces
{
    public interface ITipoDocService
    {
        Task<List<TipoDocumentoRespuestaDTO>> GetAllAsync();
        Task<TipoDocumentoRespuestaDTO?> GetByIdAsync(int Id_tipo);
        Task<TipoDocumentoRespuestaDTO> CreateAsync(TipoDocumentoCrearDTO dto);
        Task<bool> UpdateAsync(int Id_tipo, TipoDocumentoActualizarDTO dto);
        Task<bool> DeleteAsync(int Id_tipo);
    }
}
