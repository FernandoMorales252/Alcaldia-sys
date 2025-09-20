using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlcaldiaApi.Entidades
{
    //Anotacion para mapear la tabla
    [ Table ("tipodocumento")]
    public class TipoDocumento
    {
        [Key]
        //Anotacion para mapear la columna id_tipo_documento
        [Column("id_tipo_documento")]
        public int Id_tipo { get; set; }
        [Column("nombre_tipo")]
        public string Nombre { get; set; } = "";

        public ICollection<Documento> Documentos { get; set; } = new List<Documento>();
    }
}