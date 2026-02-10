using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Conversor_Monedas_Api.Entities
{
    public class Conversion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConversionId { get; set; }  // Id conversión

        [Required]
        public int UsuarioId { get; set; }  // Relación con el usuario que realizó la conversión

        [Required]
        public required string MonedaOrigen { get; set; }  // Relación con la moneda de origen

        [Required]
        public required string MonedaDestino { get; set; }  // Relación con la moneda de destino

        [Required]
        public decimal MontoOriginal { get; set; }  // Monto a convertir
        public decimal MontoConvertido { get; set; }  // Monto convertido

        [Required]//
        public DateTime FechaConversion { get; set; }  // Fecha de la conversión

        // Relaciones con otras entidades
        [ForeignKey("UsuarioId")]
        public required virtual Usuario Usuario { get; set; }
    }
}
