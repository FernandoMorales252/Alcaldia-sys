using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlcaldiaApi.Entidades
{
    [Table("usuarios")]
    public class usuario
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; } = "";
        [Column("Email")]
        public string Email { get; set; } = "";
        [Column("PasswordHash")]
        public string PasswordHash { get; set; } = "";
        [Column("RolId")]
        public int RolId { get; set; }
        public Rol Rol { get; set; } = null!;
    }
}
