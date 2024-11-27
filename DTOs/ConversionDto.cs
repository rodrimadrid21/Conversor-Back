namespace Conversor_Monedas_Api.DTOs
{
    public class ConversionDto
    {
        public int ConversionId { get; set; }
        public required int UsuarioId { get; set; }
        public required string FromCurrency { get; set; }
        public required string ToCurrency { get; set; }
        public string? FromCurrencySymbol { get; set; } // Cambiar a nullable
        public string? ToCurrencySymbol { get; set; } // Cambiar a nullable
        public required decimal Amount { get; set; }
        public decimal Result { get; set; }
        public DateTime Date { get; set; }
    }
}
