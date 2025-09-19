using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlcaldiaApi.Entidades
{
    [Table("municipio")]
    public class Municipio
    {
        [Key]
        [Column("id_municipio")]
        public int Id_Municipio { get; set; }
      
        [Column("nombre_municipio")]
        public string Nombre_Municipio { get; set; } = "";
    }
}