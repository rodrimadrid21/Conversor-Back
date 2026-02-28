using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Conversor_Monedas_Api.Entities
{
    public class Moneda
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  

        [StringLength(10)]
        [Required]
        public required string Codigo { get; set; } 

        [StringLength(100)]
        [Required]
        public required string Leyenda { get; set; } 

        [StringLength(10)]
        [Required]
        public required string Simbolo { get; set; } 

        [Required]
        public decimal IndiceConvertibilidad { get; set; }  // Relación con el dólar (Ej. 0.002 para ARS)

        public bool IsActive { get; set; } = true; 
    }
}
