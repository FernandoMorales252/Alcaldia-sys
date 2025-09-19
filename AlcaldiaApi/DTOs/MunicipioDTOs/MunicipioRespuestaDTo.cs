using System.ComponentModel.DataAnnotations.Schema;

namespace AlcaldiaApi.DTOs.MunicipioDTOs
{
    public class MunicipioRespuestaDTo
    {
        public int Id_Municipio { get; set; }

        public string Nombre_Municipio { get; set; } = "";
    }
}
