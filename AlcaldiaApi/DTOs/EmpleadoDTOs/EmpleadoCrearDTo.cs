using AlcaldiaApi.Entidades;

namespace AlcaldiaApi.DTOs.EmpleadoDTOs
{
    public class EmpleadoCrearDTo
    {
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public DateTime Fecha_contratacion { get; set; }
        public int Estado { get; set; }
        public int CargoId { get; set; }
        public int MunicipioId { get; set; }
    }
}
