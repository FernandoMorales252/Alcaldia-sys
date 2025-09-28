namespace AlcaldiaApi.DTOs.QuejaDTOs
{
    public class QuejaCrearDTo
    {
        public string Titulo { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public DateTime Fecha_Registro { get; set; }
        public string Tipo { get; set; } = "";
        public string Nivel { get; set; } = "";
        public int MunicipioId { get; set; }
    }
}
