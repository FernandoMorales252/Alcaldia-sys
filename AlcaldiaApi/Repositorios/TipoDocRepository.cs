using AlcaldiaApi.Datos;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlcaldiaApi.Repositorios
{
    public class TipoDocRepository : ITipoDocRepository
    {
        private readonly AppDbContext _context;
        public TipoDocRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TipoDocumento>> GetAllAsync()
            => await _context.Tipos.AsNoTracking().ToListAsync();

        public async Task<TipoDocumento?> GetByIdAsync(int Id_tipo)
            => await _context.Tipos.FindAsync(Id_tipo);

        public async Task<TipoDocumento> AddAsync(TipoDocumento entity)
        {
            _context.Tipos.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<bool> UpdateAsync(TipoDocumento entity)
        {
            _context.Tipos.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Tipos.FindAsync(id);
            if (existing == null) return false;
            _context.Tipos.Remove(existing);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
