using AlcaldiaApi.Interfaces;
using AlcaldiaApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlcaldiaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUsuarioRepository usuarioRepository;

        public UserController(IUsuarioRepository pUsuarioRepository)
        {
            usuarioRepository = pUsuarioRepository;
        }

        [HttpGet("usuarios")]
        [Authorize] // Solo usuarios autenticados
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await usuarioRepository.GetAllUsuariosAsync();
            return Ok(usuarios);
        }
    }
}
