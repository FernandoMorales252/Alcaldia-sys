using AlcaldiaApi.Datos;
using AlcaldiaApi.DTOs.DashboardDTOs;
using AlcaldiaApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlcaldiaApi.Repositorios
{
   
    public class DashboardRepository : IDashboardRepository
    {
        // Asegúrate de que tu contexto de base de datos se llame ApplicationDbContext
        private readonly AppDbContext _context;

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardDataDTO> GetAggregatedDataAsync()
        {
            var data = new DashboardDataDTO();

            // 1. Empleados por Cargo
            data.EmpleadosPorCargo = await _context.Empleados
                // Asume que Empleado tiene una propiedad de navegación 'Cargo'
                .Include(e => e.Cargo)
                .GroupBy(e => e.Cargo.Nombre_cargo)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            // 2. Empleados por Municipio
            data.EmpleadosPorMunicipio = await _context.Empleados
                .Include(e => e.Municipio)
                .GroupBy(e => e.Municipio.Nombre_Municipio)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            // 3. Total Empleados Activos
            data.TotalEmpleadosActivos = await _context.Empleados.CountAsync(e => e.Estado == "activo");

            // 4. Proyectos por Estado
            data.ProyectosPorEstado = await _context.Proyectos
                .GroupBy(p => p.Estado)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            // 5. Presupuesto Total por Municipio
            data.PresupuestoPorMunicipio = await _context.Proyectos
                .Include(p => p.Municipio)
                .GroupBy(p => p.Municipio.Nombre_Municipio)
                .Select(g => new { Nombre = g.Key, Presupuesto = g.Sum(p => p.Presupuesto) })
                .ToDictionaryAsync(x => x.Nombre, x => x.Presupuesto);

            // 6. Inventario por Estado (Contando la cantidad total de ítems en ese estado)
            data.InventarioPorEstado = await _context.Inventarios
                .GroupBy(i => i.Estado)
                .Select(g => new { Estado = g.Key, Cantidad = g.Sum(i => i.Cantidad) })
                .ToDictionaryAsync(x => x.Estado, x => x.Cantidad);

            // 7. Inventario Crítico (Top 5 con menor cantidad)
            data.TopItemsAgotados = await _context.Inventarios
                .Where(i => i.Estado == "disponible") // Solo los disponibles
                .OrderBy(i => i.Cantidad)
                .Take(5)
                .Select(i => new ItemCriticoDTO { NombreItem = i.Nombre_item, Cantidad = i.Cantidad })
                .ToListAsync();

            // 8. Total de Ítems en Baja
            data.TotalItemsEnBaja = await _context.Inventarios.Where(i => i.Estado == "baja").SumAsync(i => i.Cantidad);


            // 9. Quejas por Tipo
            data.QuejasPorTipo = await _context.Quejas
                .GroupBy(q => q.Tipo)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            // 10. Total Documentos Vigentes
            data.TotalDocumentosVigentes = await _context.Documentos.CountAsync(d => d.Estado == "vigente");

            return data;
        }
    }
}
