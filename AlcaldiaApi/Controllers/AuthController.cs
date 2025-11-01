using AlcaldiaApi.DTOs.UsuarioDTOs;
using AlcaldiaApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] UsuarioRegistroDTO dto)
        {
            var result = await _authService.RegistrarAsync(dto);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result == null) return Unauthorized("Credenciales inválidas");
            return Ok(result);
        }
    }
}
