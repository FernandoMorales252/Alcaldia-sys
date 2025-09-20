using AlcaldiaApi.Datos;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlcaldiaApi.Repositorios
{
    public class DocumentoRepository : IDocumentoRepository
    {
        private readonly AppDbContext _context; // Inyeccion de AppDbContext
        public DocumentoRepository(AppDbContext context)
        {
            _context = context;
        }

        // Implememtacion de obtener todos los tipos de documento definido en ITipoDocRepository
        public async Task<List<Documento>> GetAllAsync()
            => await _context.Documentos.AsNoTracking().ToListAsync();

        // Implememtacion de obtener por Id los tipos de documento definido en ITipoDocRepository
        public async Task<Documento?> GetByIdAsync(int Id_documento)
            => await _context.Documentos.FindAsync(Id_documento);

        // Implememtacion de crear los tipos de documento definido en ITipoDocRepository
        public async Task<Documento> AddAsync(Documento entity)
        {
            _context.Documentos.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // Implememtacion de actualizar los tipos de documento definido en ITipoDocRepository
        public async Task<bool> UpdateAsync(Documento entity)
        {
            _context.Documentos.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        // Implememtacion de eliminar los tipos de documento definido en ITipoDocRepository
        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Documentos.FindAsync(id);
            if (existing == null) return false;
            _context.Documentos.Remove(existing);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
