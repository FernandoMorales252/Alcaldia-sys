using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlcaldiaApi.Entidades
{
    [Table ("documentoarchivo")]
    public class Documento
    {
        [Key]
        [Column("id_documento")]
        public int Id_documento { get; set; }
        [Column("numero_documento")]
        public string Numero_documento { get; set; } = "";
        [Column("fecha_emision")]
        public DateTime Fecha_emision { get; set; }
        [Column("nombre_persona")]
        public string Propietario { get; set; } = "";
        [Column("detalles")]
        public string Detalles { get; set; } = "";
        [Column("estado")]
        public string Estado { get; set; }

        [Column("id_tipo_documento")]
        public int TipoDocumentoId { get; set; } //Para hacer la relacion se hace como este ejemplo linea 24 y 25
        public TipoDocumento TipoDocumento { get; set; } = null!; //TipoDocumento es el nombre de la entidad a la que se relaciona

        [Column("id_municipio")] //Con esta anotacion asegurar el nombre correcto de cada columna
        public int MunicipioId { get; set; }
        public Municipio Municipio { get; set; } = null!;


    }
}