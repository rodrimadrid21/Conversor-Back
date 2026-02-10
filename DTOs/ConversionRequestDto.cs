namespace Conversor_Monedas_Api.DTOs
{
    public class ConversionRequestDto
    {
        public string FromCurrency { get; set; } = string.Empty;
        public string ToCurrency { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
