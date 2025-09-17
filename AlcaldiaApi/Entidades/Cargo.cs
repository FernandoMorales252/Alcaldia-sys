using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlcaldiaApi.Entidades
{
    [Table("cargo")]
    public class Cargo
    {
        [Key]
        [Column("id_cargo")]
        public int Id_Cargo { get; set; }

        [Column("nombre_cargo")]
        public string Nombre_cargo { get; set; } = "";

        [Column("descripcion")]
        public string Descripcion { get; set; } = "";

    }
}