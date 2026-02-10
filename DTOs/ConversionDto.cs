namespace Conversor_Monedas_Api.DTOs
{
    public class ConversionDto
    {
        public int ConversionId { get; set; }

        // 👇 sacamos "required" porque el UserId lo sacamos del token
        public int UsuarioId { get; set; }

        public required string FromCurrency { get; set; }
        public required string ToCurrency { get; set; }

        public string? FromCurrencySymbol { get; set; }
        public string? ToCurrencySymbol { get; set; }

        public required decimal Amount { get; set; }

        public decimal Result { get; set; }

        public DateTime Date { get; set; }
    }

}
