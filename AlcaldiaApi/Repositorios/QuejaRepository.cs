using AlcaldiaApi.Datos;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlcaldiaApi.Repositorios
{
    public class QuejaRepository : IQuejaRepository
    {
        private readonly AppDbContext _context;
        public QuejaRepository(AppDbContext context) { _context = context; } // Constructor que recibe el contexto

        // Implementacion de los metodos CRUD

        //Listar todos los empleados
        public async Task<List<Queja>> GetAllAsync()
            => await _context.Quejas.AsNoTracking().ToListAsync();

        // Buscar empleado por Id
        public async Task<Queja?> GetByIdAsync(int Id_queja)
            => await _context.Quejas.FindAsync(Id_queja);

        // Agregar nuevo empleado
        public async Task<Queja> AddAsync(Queja entity)
        {
            _context.Quejas.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        //Actualizar empleado
        public async Task<bool> UpdateAsync(Queja entity)
        {
            _context.Quejas.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        //Eliminar empleado
        public async Task<bool> DeleteAsync(int Id_queja)
        {
            var existing = await _context.Quejas.FindAsync(Id_queja);
            if (existing == null) return false;
            _context.Quejas.Remove(existing);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
