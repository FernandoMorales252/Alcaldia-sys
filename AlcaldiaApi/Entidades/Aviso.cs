using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlcaldiaApi.Entidades
{
    [Table("aviso")]
    public class Aviso
    {
        [Key]
        [Column("id_aviso")]
        public int Id_aviso { get; set; }
        [Column("titulo")]
        public string Titulo { get; set; } = "";
        [Column("descripcion")]
        public string Descripcion { get; set; } = "";
        [Column("fecha_registro")]
        public DateTime Fecha_Registro { get; set; }
        [Column("tipo")]
        public string Tipo { get; set; } = "";
        [Column("id_municipio")]
        public int MunicipioId { get; set; }

        public Municipio Municipio { get; set; } = null!;
    }
}
