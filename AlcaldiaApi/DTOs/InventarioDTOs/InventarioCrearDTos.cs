using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlcaldiaApi.DtOs.InventarioDTOs
{
    public class InventarioCrearDTO
    {
        public string Nombre_item { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public int Cantidad { get; set; }
        public DateTime Fecha_ingreso { get; set; }
        public string Estado { get; set; }
        public int MunicipioId { get; set; }
        public string? ImagenBase64 { get; set; }
    }
}