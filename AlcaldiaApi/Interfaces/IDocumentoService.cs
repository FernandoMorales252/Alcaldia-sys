using AlcaldiaApi.DtOs.DocumentoDtos;


namespace AlcaldiaApi.Interfaces
{
    public interface IDocumentoService
    {
        Task<List<DocumentoARespuestaDTO>> GetAllAsync();
        Task<DocumentoARespuestaDTO?> GetByIdAsync(int Id_documento);
        Task<DocumentoARespuestaDTO> CreateAsync(DocumentoCrearDTO dto);
        Task<bool> UpdateAsync(int Id_documento, DocumentoActualizarDTO dto);
        Task<bool> DeleteAsync(int Id_documento);
        
    }
}
