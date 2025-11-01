using AlcaldiaApi.DTOs.UsuarioDTOs;
using AlcaldiaApi.Interfaces;

namespace ComedorMariscos.Servicios
{
    
    public class AuthService : IAuthService
    {
        public async Task<UsuarioRespuestaDTO> RegistrarAsync(UsuarioRegistroDTO dto)
        {
            // ⚠️ Aquí deberías guardar el usuario en la BD con tu repositorio
            // Por ahora simulamos la creación de usuario y devolución de token
            return await Task.FromResult(new UsuarioRespuestaDTO
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Token = "fake-token-registrado" // aquí luego generas el JWT real
            });
        }

        public async Task<UsuarioRespuestaDTO?> LoginAsync(UsuarioLoginDTO dto)
        {
            // ⚠️ Aquí deberías validar contra la BD (repositorio de usuarios)
            // Por ahora simulamos validación de credenciales
            if (dto.Email == "admin@test.com" && dto.Password == "1234")
            {
                return await Task.FromResult(new UsuarioRespuestaDTO
                {
                    Nombre = "admin",
                    Email = dto.Email,
                    Token = "fake-token-login" // aquí luego generas el JWT real
                });
            }

            // Si las credenciales no son válidas
            return null;
        }
    }
}

