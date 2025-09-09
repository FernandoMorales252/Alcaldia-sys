namespace AlcaldiaApi.Entidades
{
    public class Inventario
    {
        public int Id_inventario { get; set; }
    
        public string Nombre_item { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal Precio_unitario { get; set; }
        public DateTime Fecha_ingreso { get; set; }
        public int Estado { get; set; }

        public int MunicipioId { get; set; }
        public Municipio Municipio { get; set; } = null!;
    }
}