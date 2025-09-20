using AlcaldiaApi.Datos;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlcaldiaApi.Repositorios
{
    public class TipoDocRepository : ITipoDocRepository // Inyeccion de ITipoDocRepository
    {
        private readonly AppDbContext _context; // Inyeccion de AppDbContext
        public TipoDocRepository(AppDbContext context)
        {
            _context = context;
        }

        // Implememtacion de obtener todos los tipos de documento definido en ITipoDocRepository
        public async Task<List<TipoDocumento>> GetAllAsync()
            => await _context.Tipos.AsNoTracking().ToListAsync();

        // Implememtacion de obtener por Id los tipos de documento definido en ITipoDocRepository
        public async Task<TipoDocumento?> GetByIdAsync(int Id_tipo)
            => await _context.Tipos.FindAsync(Id_tipo);

        // Implememtacion de crear los tipos de documento definido en ITipoDocRepository
        public async Task<TipoDocumento> AddAsync(TipoDocumento entity)
        {
            _context.Tipos.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // Implememtacion de actualizar los tipos de documento definido en ITipoDocRepository
        public async Task<bool> UpdateAsync(TipoDocumento entity)
        {
            _context.Tipos.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        // Implememtacion de eliminar los tipos de documento definido en ITipoDocRepository
        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Tipos.FindAsync(id);
            if (existing == null) return false;
            _context.Tipos.Remove(existing);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
