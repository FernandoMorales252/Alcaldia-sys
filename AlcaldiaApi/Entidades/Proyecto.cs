using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlcaldiaApi.Entidades
{
    [Table("proyecto")]
    public class Proyecto
    {
        [Key]
        [Column("id_proyecto")]
        public int Id_proyecto { get; set; }
        [Column("nombre")]
        public string Nombre { get; set; } = "";
        [Column("descripcion")]
        public string Descripcion { get; set; } = "";
        [Column("fecha_inicio")]
        public DateTime Fecha_inicio { get; set; }
        [Column("fecha_fin")]
        public DateTime Fecha_fin { get; set; }
        [Column("presupuesto")]
        public decimal Presupuesto { get; set; }
        [Column("estado")]
        public string Estado { get; set; } = "";
        [Column("id_municipio")]
        public int MunicipioId { get; set; }

        public Municipio Municipio { get; set; } = null!;
    }
}