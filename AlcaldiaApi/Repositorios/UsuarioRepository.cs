using AlcaldiaApi.Datos;
using AlcaldiaApi.DTOs.UsuarioDTOs;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComedorMariscos.Repositorios
{
    
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Email == email);
        }


        public async Task<usuario> AddAsync(usuario Usuario)
        {
            _context.Usuarios.Add(Usuario);
            await _context.SaveChangesAsync();
            return Usuario;
        }

        public async Task<List<UsuarioListadoDTO>> GetAllUsuariosAsync()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Rol)
                .ToListAsync();

            return usuarios.Select(u => new UsuarioListadoDTO
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Email = u.Email,
                Rol = u.Rol?.Nombre ?? "Sin Rol"

            }).ToList();


        }



    }

}

