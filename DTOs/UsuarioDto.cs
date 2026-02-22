using System.ComponentModel.DataAnnotations;
using Conversor_Monedas_Api.Enum;


namespace Conversor_Monedas_Api.DTOs
{
    public class UsuarioDto
    {
        public int UserId { get; set; }
        public required string FirstName { get; set; } 

        public required string LastName { get; set; } 

        public required string UserName { get; set; } 

        public string? Password { get; set; } 

        public SuscripcionEnum Type { get; set; } = SuscripcionEnum.Free;

        public DateTime SubscriptionStartDate { get; set; } = DateTime.UtcNow;
    }
}
