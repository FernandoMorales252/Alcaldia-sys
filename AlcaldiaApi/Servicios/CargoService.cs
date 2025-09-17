using AlcaldiaApi.DTOs.CargoDTOs;
using AlcaldiaApi.Entidades;
using AlcaldiaApi.Interfaces;

namespace AlcaldiaApi.Servicios
{
    public class CargoService : ICargoService
    {
        private readonly ICargoRepository _repo;
        public CargoService(ICargoRepository repo) => _repo = repo;
        public async Task<List<CargoRespuestaDTo>> GetAllAsync() =>
            (await _repo.GetAllAsync()).Select(x => new CargoRespuestaDTo
            {
                Id_Cargo = x.Id_Cargo,
                Nombre_cargo = x.Nombre_cargo,
                Descripcion = x.Descripcion
            }).ToList();
        public async Task<CargoRespuestaDTo?> GetByIdAsync(int id)
        {
            var x = await _repo.GetByIdAsync(id);
            return x == null ? null : new CargoRespuestaDTo
            {
                Id_Cargo = x.Id_Cargo,
                Nombre_cargo = x.Nombre_cargo,
                Descripcion = x.Descripcion
            };
        }
        public async Task<CargoRespuestaDTo> CreateAsync(CargoCrearDTo dto)
        {
            var entity = new Cargo { Nombre_cargo = dto.Nombre_cargo.Trim(), Descripcion = dto.Descripcion.Trim() };
            var saved = await _repo.AddAsync(entity);
            return new CargoRespuestaDTo { Id_Cargo = saved.Id_Cargo, Nombre_cargo = saved.Nombre_cargo, Descripcion = saved.Descripcion };
        }

        public async Task<bool> UpdateAsync(int Id_Cargo, CargoActualizarDTo dto)
        {
            var current = await _repo.GetByIdAsync(Id_Cargo);
            if (current == null) return false;
            current.Nombre_cargo = dto.Nombre_cargo.Trim();
            current.Descripcion = dto.Descripcion.Trim();
            return await _repo.UpdateAsync(current);
        }

        public Task<bool> DeleteAsync(int Id_Cargo) => _repo.DeleteAsync(Id_Cargo);
    }
}
