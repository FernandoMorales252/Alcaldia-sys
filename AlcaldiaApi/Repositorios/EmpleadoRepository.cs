using AlcaldiaApi.Datos;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlcaldiaApi.Repositorios
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly AppDbContext _context; // Contexto de la base de datos
        public EmpleadoRepository(AppDbContext context) { _context = context; } // Constructor que recibe el contexto

        // Implementacion de los metodos CRUD

        //Listar todos los empleados
        public async Task<List<Empleado>> GetAllAsync()
            => await _context.Empleados.AsNoTracking().ToListAsync();

        // Buscar empleado por Id
        public async Task<Empleado?> GetByIdAsync(int Id_empleado)
            => await _context.Empleados.FindAsync(Id_empleado);

        // Agregar nuevo empleado
        public async Task<Empleado> AddAsync(Empleado entity)
        {
            _context.Empleados.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        //Actualizar empleado
        public async Task<bool> UpdateAsync(Empleado entity)
        {
            _context.Empleados.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        //Eliminar empleado
        public async Task<bool> DeleteAsync(int Id_empleado)
        {
            var existing = await _context.Empleados.FindAsync(Id_empleado);
            if (existing == null) return false;
            _context.Empleados.Remove(existing);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
