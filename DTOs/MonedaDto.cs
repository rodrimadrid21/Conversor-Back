namespace Conversor_Monedas_Api.DTOs
{
    public class MonedaDto
    {
        public required int CurrencyId { get; set; }
        public required string Code { get; set; } // ("ARS", "USDT", "BTC", "ETH", "LTC")

        public required string Legend { get; set; } // ("Pesos Argentinos")

        public required string Symbol { get; set; } // ("$", "₮", "₿", "Ξ", "Ł")

        public required decimal ConvertibilityIndex { get; set; } 
    }
}
