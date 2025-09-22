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

        public ICollection<Documento> Documentos { get; set; } = new List<Documento>();
        //Si su tabla esta relacionada con esta o otra solamente copiar el public collection pegarlo abajo de la ultima linea y cambiar lo que hay en los <    >
        //por el nombre de la entidad ejemplo .. public ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();

        public ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
        public ICollection<Proyecto> Proyectos { get; set; } = new List<Proyecto>();
    }
}