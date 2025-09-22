using AlcaldiaApi.Datos;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlcaldiaApi.Repositorios
{
    public class ProyectoRepository : IProyectoRepository
    {
        private readonly AppDbContext _context;
        public ProyectoRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Proyecto>> GetAllAsync()
            => await _context.Proyectos.AsNoTracking().ToListAsync();
        public async Task<Proyecto?> GetByIdAsync(int Id_proyecto)
            => await _context.Proyectos.FindAsync(Id_proyecto);
        public async Task<Proyecto> AddAsync(Proyecto entity)
        {
            _context.Proyectos.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<bool> UpdateAsync(Proyecto entity)
        {
            _context.Proyectos.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Proyectos.FindAsync(id);
            if (existing == null) return false;
            _context.Proyectos.Remove(existing);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
