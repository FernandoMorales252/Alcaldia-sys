using AlcaldiaApi.DTOs.UsuarioDTOs;
using AlcaldiaApi.Entidades;

namespace AlcaldiaApi.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<usuario?> GetByEmailAsync(string email);
        Task<usuario> AddAsync(usuario usuario);
        Task<List<UsuarioListadoDTO>> GetAllUsuariosAsync();
    }
}
