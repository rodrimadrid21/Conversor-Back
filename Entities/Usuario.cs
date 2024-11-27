using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Conversor_Monedas_Api.Enum;

namespace Conversor_Monedas_Api.Entities
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [StringLength(50)]
        [Required]
        public required string FirstName { get; set; }

        [StringLength(50)]
        [Required]
        public required string LastName { get; set; }

        [StringLength(50)]
        [Required]
        public required string UserName { get; set; }

        [StringLength(50)]
        [DataType(DataType.Password)]
        [Required]
        public required string Password { get; set; }

        public UsuarioEnum Role { get; set; }// = UsuarioEnum.user; // Rol del usuario (por defecto 'user')

        public SuscripcionEnum Type; // Tipo de suscripción

        public DateTime SubscriptionStartDate { get; set; } // Fecha de inicio de la suscripción

        public bool IsActive { get; set; } = true;
        public int ConversionCount { get; set; } = 0; // Inicializado en 0
    }
}
