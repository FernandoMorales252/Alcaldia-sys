using AlcaldiaApi.Datos;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlcaldiaApi.Repositorios
{
    public class InventarioRepository : IInventarioRepository
    {
        private readonly AppDbContext _context;
        public InventarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Inventario>> GetAllAsync()
          => await _context.Inventarios.AsNoTracking().ToListAsync();

        public async Task<Inventario?> GetByIdAsync(int Id_inventario)
            => await _context.Inventarios.FindAsync(Id_inventario);

        public async Task<Inventario> AddAsync(Inventario entity)
        {
            _context.Inventarios.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(Inventario entity)
        {
            _context.Inventarios.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Inventarios.FindAsync(id);
            if (existing == null) return false;
            _context.Inventarios.Remove(existing);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
