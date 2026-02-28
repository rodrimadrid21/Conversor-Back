using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conversor_Monedas_Api.Entities
{
    public class Conversion
    {

        // PK
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConversionId { get; set; }

        // FK
        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public required Usuario Usuario { get; set; } 

        // ---

        [Required]
        public required string MonedaOrigen { get; set; }

        [Required]
        public required string MonedaDestino { get; set; }

        [Required]
        public decimal MontoOriginal { get; set; }

        public decimal MontoConvertido { get; set; }

        [Required]
        public DateTime FechaConversion { get; set; }
        
    }
}
