namespace Conversor_Monedas_Api.DTOs
{
    public class MonedaDto
    {
        public required int CurrencyId { get; set; }
        public required string Code { get; set; } // Código de la moneda (e.g., USD, EUR)

        public required string Legend { get; set; } // Descripción o leyenda de la moneda

        public required string Symbol { get; set; } // Símbolo de la moneda (e.g., $, €)

        public required decimal ConvertibilityIndex { get; set; } // Índice de convertibilidad
    }
}
