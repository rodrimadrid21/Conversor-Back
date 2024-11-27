using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Conversor_Monedas_Api.Enum;

namespace Conversor_Monedas_Api.Entities
{
    public class Suscripcion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // Id de la suscripción

        [Required]
        public SuscripcionEnum Tipo { get; set; }  // Tipo ("Free", "Trial", "Pro")

        [Required]
        public bool MaximoConversiones { get; set; }  // Número máximo de conversiones permitidas
    }
}
