using AlcaldiaApi.Entidades;

namespace AlcaldiaApi.DTOs.EmpleadoDTOs
{
    public class EmpleadoActualizarDTo
    {
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public DateTime Fecha_contratacion { get; set; }
        public int Estado { get; set; }
        public int CargoId { get; set; }
        public Cargo Cargo { get; set; } = null!;
    }
}
