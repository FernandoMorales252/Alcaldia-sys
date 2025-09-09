namespace AlcaldiaApi.Entidades
{
    public class Proyecto
    {
        public int Id_proyecto { get; set; }

        public string Nombre { get; set; } = "";

        public string Descripcion { get; set; } = "";

        public DateTime Fecha_inicio { get; set; }

        public DateTime? Fecha_fin { get; set; }

        public decimal Presupuesto { get; set; }

        public string Estado { get; set; } = "";

        public int MunicipioId { get; set; }

        public Municipio Municipio { get; set; } = null!;
    }
}