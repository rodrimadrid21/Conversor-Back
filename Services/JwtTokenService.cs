using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Conversor_Monedas_Api.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                // Validación del token
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true, // Verifica si el token ha expirado
                    IssuerSigningKey = new SymmetricSecurityKey(key) // La clave secreta con la que se firmó el token
                }, out SecurityToken validatedToken);

                // Si no hubo errores, el token es válido
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error durante la validación del token: {ex.Message}");
            }

            // Devuelve falso si ocurre alguna excepción
            return false;
        }

    }
}