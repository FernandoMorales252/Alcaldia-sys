using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlcaldiaApi.Entidades
{
    [Table ("inventario")]
    public class Inventario
    {
        [Key]
        [Column("id_inventario")]
        public int Id_inventario { get; set; }

        [Column("nombre_item")]
        public string Nombre_item { get; set; } = "";

        [Column("descripcion")]
        public string Descripcion { get; set; } = "";

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("fecha_ingreso")]
        public DateTime Fecha_ingreso { get; set; }

        [Column("estado")]
        public string Estado { get; set; }

        [Column("id_municipio")]
        public int MunicipioId { get; set; }
        public Municipio Municipio { get; set; } = null!;
    }
}