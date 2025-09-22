using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlcaldiaApi.Entidades
{
    [Table("empleado")]
    public class Empleado
    {
        [Key]
        [Column("id_empleado")]
        public int Id_empleado { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; } = "";

        [Column("apellido")]
        public string Apellido { get; set; } = "";

        [Column("fecha_contratacion")]
        public DateTime Fecha_contratacion { get; set; }

        [Column("estado")]
        public int Estado { get; set; }

        [Column("id_cargo")]
        public int CargoId { get; set; }
        public Cargo Cargo { get; set; } = null!;

        [Column("id_municipio")]
        public int MunicipioId { get; set; }
        public Municipio Municipio { get; set; } = null!;


    }
}