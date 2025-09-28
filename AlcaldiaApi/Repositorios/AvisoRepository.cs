using AlcaldiaApi.Datos;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlcaldiaApi.Repositorios
{
    public class AvisoRepository : IAvisoRepository
    {
        private readonly AppDbContext _context; 
        public AvisoRepository(AppDbContext context) { _context = context; } // Constructor que recibe el contexto

        // Implementacion de los metodos CRUD

        //Listar todos los empleados
        public async Task<List<Aviso>> GetAllAsync()
            => await _context.Avisos.AsNoTracking().ToListAsync();

        // Buscar empleado por Id
        public async Task<Aviso?> GetByIdAsync(int Id_aviso)
            => await _context.Avisos.FindAsync(Id_aviso);

        // Agregar nuevo empleado
        public async Task<Aviso> AddAsync(Aviso entity)
        {
            _context.Avisos.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        //Actualizar empleado
        public async Task<bool> UpdateAsync(Aviso entity)
        {
            _context.Avisos.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        //Eliminar empleado
        public async Task<bool> DeleteAsync(int Id_aviso)
        {
            var existing = await _context.Avisos.FindAsync(Id_aviso);
            if (existing == null) return false;
            _context.Avisos.Remove(existing);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
