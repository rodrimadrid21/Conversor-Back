using System.ComponentModel.DataAnnotations;
using Conversor_Monedas_Api.Enum;


namespace Conversor_Monedas_Api.DTOs
{
    public class UsuarioDto
    {
        public required string FirstName { get; set; } // Nombre

        public required string LastName { get; set; } // Apellido

        public required string UserName { get; set; } // Nombre de usuario

        [DataType(DataType.Password)]
        public string Password { get; set; } // Contraseña

        public UsuarioEnum Role { get; set; } = UsuarioEnum.user; // Rol del usuario

        public SuscripcionEnum Type { get; set; } = SuscripcionEnum.Free;// Tipo de suscripción

        public DateTime SubscriptionStartDate { get; set; } = DateTime.UtcNow; // Fecha de inicio de la suscripción
    }
}
