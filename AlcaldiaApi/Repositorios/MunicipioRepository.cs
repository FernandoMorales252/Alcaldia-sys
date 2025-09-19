using AlcaldiaApi.Datos;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlcaldiaApi.Repositorios
{
    public class MunicipioRepository : IMunicipioRepository
    {
        private readonly AppDbContext _context;
        public MunicipioRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Municipio>> GetAllAsync()
            => await _context.Municipios.AsNoTracking().ToListAsync();
        public async Task<Municipio?> GetByIdAsync(int Id_Municipio)
            => await _context.Municipios.FindAsync(Id_Municipio);
        public async Task<Municipio> AddAsync(Municipio entity)
        {
            _context.Municipios.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<bool> UpdateAsync(Municipio entity)
        {
            _context.Municipios.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Municipios.FindAsync(id);
            if (existing == null) return false;
            _context.Municipios.Remove(existing);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
