using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlcaldiaApi.Entidades
{
    [Table("queja")]
    public class Queja
    {
        [Key]
        [Column("id_queja")]
        public int Id_queja { get; set; }
        [Column("titulo")]
        public string Titulo { get; set; } = "";
        [Column("descripcion")]
        public string Descripcion { get; set; } = "";
        [Column("fecha_registro")]
        public DateTime Fecha_Registro { get; set; }
        [Column("tipo")]
        public string Tipo { get; set; } = "";
        [Column("nivel")]
        public string Nivel { get; set; } = "";
        [Column("id_municipio")]
        public int MunicipioId { get; set; }

        public Municipio Municipio { get; set; } = null!;
    }
}
