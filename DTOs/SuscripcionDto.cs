using Conversor_Monedas_Api.Enum;

namespace Conversor_Monedas_Api.DTOs
{
    public class SuscripcionDto
    {
        public int Id { get; set; }
        public SuscripcionEnum Type { get; set; } // Tipo de suscripción (Free, Trial, Pro)
        public int ConversionLimit { get; set; } // Límite de conversiones
        public bool MonthlyReset { get; set; } // Indica si se reinicia mensualmente
    }
}
