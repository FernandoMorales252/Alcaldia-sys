using AlcaldiaApi.DTOs.EmpleadoDTOs;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;

namespace AlcaldiaApi.Servicios
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly IEmpleadoRepository _repo; //Repositorio

        public EmpleadoService(IEmpleadoRepository repo) => _repo = repo; //Inyeccion de dependencia

        //Metodo para obtener todos
        public async Task<List<EmpleadoRespuestaDTo>> GetAllAsync() =>
            (await _repo.GetAllAsync()).Select(x => new EmpleadoRespuestaDTo
            {
                Id_empleado = x.Id_empleado,
                Nombre = x.Nombre,
                Apellido = x.Apellido,
                Fecha_contratacion = x.Fecha_contratacion,
                Estado = x.Estado,
                CargoId = x.CargoId,
                MunicipioId = x.MunicipioId,

            }).ToList();

        //Metodo para obtener por Id
        public async Task<EmpleadoRespuestaDTo?> GetByIdAsync(int Id_empleado)
        {
            var x = await _repo.GetByIdAsync(Id_empleado);
            return x == null ? null : new EmpleadoRespuestaDTo
            {
                Id_empleado = x.Id_empleado,
                Nombre = x.Nombre,
                Apellido = x.Apellido,
                Fecha_contratacion = x.Fecha_contratacion,
                Estado = x.Estado,
                CargoId = x.CargoId,
                MunicipioId = x.MunicipioId,
            };
        }


        //Metodo para crear
        public async Task<EmpleadoRespuestaDTo> CreateAsync(EmpleadoCrearDTo dto)
        {
            var entity = new Empleado
            {
                Nombre = dto.Nombre.Trim(),
                Apellido = dto.Apellido.Trim(),
                Fecha_contratacion = dto.Fecha_contratacion,
                Estado = dto.Estado.Trim(),
                CargoId = dto.CargoId,
                MunicipioId = dto.MunicipioId,
            };

            var saved = await _repo.AddAsync(entity);
            return new EmpleadoRespuestaDTo
            {
                Id_empleado = saved.Id_empleado,
                Nombre = saved.Nombre,
                Apellido = saved.Apellido,
                Fecha_contratacion = saved.Fecha_contratacion,
                Estado = saved.Estado,
                CargoId = saved.CargoId,
                MunicipioId = saved.MunicipioId,
            };
        }


        //Metodo para actualizar
        public async Task<bool> UpdateAsync(int Id_empleado, EmpleadoActualizarDTo dto)
        {
            var current = await _repo.GetByIdAsync(Id_empleado);
            if (current == null) return false;
            current.Nombre = dto.Nombre.Trim();
            current.Apellido = dto.Apellido.Trim();
            current.Fecha_contratacion = dto.Fecha_contratacion;
            current.Estado = dto.Estado.Trim();
            current.CargoId = dto.CargoId;
            current.MunicipioId = dto.MunicipioId;

            return await _repo.UpdateAsync(current);
        }

        //Metodo para eliminar
        public Task<bool> DeleteAsync(int Id_empleado) => _repo.DeleteAsync(Id_empleado);
    }
}
