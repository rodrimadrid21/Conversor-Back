namespace Conversor_Monedas_Api.DTOs
{
    public class MonedaDto
    {
        public int CurrencyId { get; set; }
        public required string Code { get; set; } 

        public required string Legend { get; set; } 

        public required string Symbol { get; set; } 

        public required decimal ConvertibilityIndex { get; set; } 
    }
}
