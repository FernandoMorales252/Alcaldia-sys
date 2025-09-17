using Microsoft.EntityFrameworkCore;
using AlcaldiaApi.Datos;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;

namespace AlcaldiaApi.Repositorios
{
    public class CargoRepository : ICargoRepository
    {
        private readonly AppDbContext _context;
        public CargoRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Cargo>> GetAllAsync()
            => await _context.Cargos.AsNoTracking().ToListAsync();
        public async Task<Cargo?> GetByIdAsync(int Id_Cargo)
            => await _context.Cargos.FindAsync(Id_Cargo);
        public async Task<Cargo> AddAsync(Cargo entity)
        {
            _context.Cargos.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<bool> UpdateAsync(Cargo entity)
        {
            _context.Cargos.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Cargos.FindAsync(id);
            if (existing == null) return false;
            _context.Cargos.Remove(existing);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
