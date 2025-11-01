using AlcaldiaApi.DTOs.UsuarioDTOs;

namespace AlcaldiaApi.Interfaces
{
    
    
        public interface IAuthService
        {
            Task<UsuarioRespuestaDTO> RegistrarAsync(UsuarioRegistroDTO dto);
            Task<UsuarioRespuestaDTO> LoginAsync(UsuarioLoginDTO dto);
        }

    }



