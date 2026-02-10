using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Conversor_Monedas_Api.Enum;

namespace Conversor_Monedas_Api.Entities
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Generación automática del Id
        public int UserId { get; set; }

        [StringLength(20)]
        [Required]
        public required string FirstName { get; set; }

        [StringLength(20)]
        [Required]
        public required string LastName { get; set; }

        [StringLength(20)]
        [Required]
        public required string UserName { get; set; }

        [StringLength(20)]
        [Required]
        public required string Password { get; set; }

        public UsuarioEnum Role { get; set; } = UsuarioEnum.user;

        public SuscripcionEnum Type { get; set; } = SuscripcionEnum.Free;
        // ✅ NUEVO (nullable por ahora para no romper DB)
        public int? SuscripcionId { get; set; }

        public DateTime SubscriptionStartDate { get; set; } = DateTime.UtcNow;// Fecha de inicio de la suscripción

        public bool IsActive { get; set; } = true;
    }
}
