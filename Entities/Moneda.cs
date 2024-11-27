using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Conversor_Monedas_Api.Entities
{
    public class Moneda
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // Id de la moneda

        [StringLength(10)]
        [Required]
        public required string Codigo { get; set; }  // codigo("ARS", "USDT", "BTC", "ETH", "LTC")

        [StringLength(100)]
        [Required]
        public required string Leyenda { get; set; }  // Descripción ("Dólar Americano")

        [StringLength(10)]
        [Required]
        public required string Simbolo { get; set; }  // Símbolo de la moneda ("$", "₮", "₿", "Ξ", "Ł")

        [Required]
        public decimal IndiceConvertibilidad { get; set; }  // Relación con el dólar (Ej. 0.002 para ARS)

        public bool IsActive { get; set; } = true; // Estado activo para eliminación lógica
    }
}
