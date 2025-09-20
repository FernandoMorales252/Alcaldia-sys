using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlcaldiaApi.Entidades
{
    [ Table ("tipodocumento")]
    public class TipoDocumento
    {
        [Key]
        [Column("id_tipo_documento")]
        public int Id_tipo { get; set; }
        [Column("nombre_tipo")]
        public string Nombre { get; set; } = "";
    }
}