using AlcaldiaApi.DTOs.ProyectoDTOs;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;

namespace AlcaldiaApi.Servicios
{
    public class ProyectoService : IProyectoService
    {
        private readonly IProyectoRepository _repo;
        public ProyectoService(IProyectoRepository repo) => _repo = repo;
        public async Task<List<ProyectoRespuestaDTo>> GetAllAsync() =>
            (await _repo.GetAllAsync()).Select(x => new ProyectoRespuestaDTo
            {
                Id_Proyecto = x.Id_proyecto,
                Nombre = x.Nombre,
                Descripcion = x.Descripcion,
                Fecha_Inicio = x.Fecha_inicio,
                Fecha_Fin = x.Fecha_fin,
                Presupuesto = x.Presupuesto,
                Estado = x.Estado,
                Id_Municipio = x.MunicipioId,
            }).ToList();
        public async Task<ProyectoRespuestaDTo?> GetByIdAsync(int id)
        {
            var x = await _repo.GetByIdAsync(id);
            return x == null ? null : new ProyectoRespuestaDTo
            {
                Id_Proyecto = x.Id_proyecto,
                Nombre = x.Nombre,
                Descripcion = x.Descripcion,
                Fecha_Inicio = x.Fecha_inicio,
                Fecha_Fin = x.Fecha_inicio,
                Presupuesto = x.Presupuesto,
                Estado = x.Estado,
                Id_Municipio = x.MunicipioId,
            };
        }
        public async Task<ProyectoRespuestaDTo> CreateAsync(ProyectoCrearDTo dto)
        {
            var entity = new Proyecto { Nombre = dto.Nombre.Trim(), Descripcion = dto.Descripcion.Trim(), Fecha_inicio = dto.Fecha_Inicio, Fecha_fin = dto.Fecha_Fin, Presupuesto = dto.Presupuesto, Estado = dto.Estado.Trim(), MunicipioId = dto.Id_Municipio };
            var saved = await _repo.AddAsync(entity);
            return new ProyectoRespuestaDTo { Id_Proyecto = saved.Id_proyecto, Nombre = saved.Nombre, Descripcion = saved.Descripcion, Fecha_Inicio = saved.Fecha_inicio, Fecha_Fin = saved.Fecha_fin, Presupuesto = saved.Presupuesto, Estado = saved.Estado, Id_Municipio = saved.MunicipioId };
        }

        public async Task<bool> UpdateAsync(int Id_Proyecto, ProyectoActualizarDTo dto)
        {
            var current = await _repo.GetByIdAsync(Id_Proyecto);
            if (current == null) return false;
            current.Nombre = dto.Nombre.Trim();
            return await _repo.UpdateAsync(current);
        }

        public Task<bool> DeleteAsync(int Id_Proyecto) => _repo.DeleteAsync(Id_Proyecto);
    }
}
