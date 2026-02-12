using Conversor_Monedas_Api.Enum;

namespace Conversor_Monedas_Api.DTOs
{
    public class SuscripcionDto
    {
        public int Id { get; set; }
        public SuscripcionEnum Type { get; set; } 
        public int ConversionLimit { get; set; } 
    }
}
