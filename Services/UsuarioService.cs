using Conversor_Monedas_Api.DTOs;
using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Enum;
using Conversor_Monedas_Api.Interfaces.repositories;
using Conversor_Monedas_Api.Interfaces.services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Conversor_Monedas_Api.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IConfiguration _configuration;

        public UsuarioService(IUsuarioRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public Usuario ValidateUser(AuthenticationDto credentials)
        {
            return _repository.Authenticate(credentials.Username, credentials.Password);
        }

        public string Authenticate(AuthenticationDto credentials)
        {
            Usuario user = ValidateUser(credentials);
            if (user == null)
            {
                return null; // Usuario no autenticado
            }

            return GenerateJwtToken(user);
        }

        public string GenerateJwtToken(Usuario user)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),         // "sub"
        new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName ?? ""),     // "given_name"
        new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName ?? ""),     // "family_name"
        new Claim("subscriptionType", user.Type.ToString()),
    };

            var jwtToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }


        public int GetUserIdFromContext(ClaimsPrincipal user)
        {
            // Intentar con "sub" estándar de JWT y con NameIdentifier por si algún mapeo se mete
            var userIdClaim =
                user?.FindFirst(JwtRegisteredClaimNames.Sub) ??          // "sub"
                user?.FindFirst(ClaimTypes.NameIdentifier) ??            // por si ASP.NET lo mapea
                user?.FindFirst("sub");                                  // fallback literal

            if (userIdClaim != null)
            {
                Console.WriteLine($"[GetUserIdFromContext] UserId desde claims: {userIdClaim.Value}");
                return int.Parse(userIdClaim.Value);
            }

            Console.WriteLine("[GetUserIdFromContext] No se encontró el claim de usuario (sub / nameidentifier)");
            throw new UnauthorizedAccessException("Usuario no autenticado.");
        }

        public List<UsuarioDto> GetAllUsers()
        {
            var users = _repository.GetAllUsers();
            return users.Select(u => new UsuarioDto
            {
                UserName = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Type = u.Type,
                SubscriptionStartDate = u.SubscriptionStartDate
            }).ToList();
        }

        public int RegisterUser(UsuarioDto userDto)
        {
            // Verificar si el usuario ya existe
            var existingUser = _repository.GetUserByUsername(userDto.UserName);
            if (existingUser != null)
            {
                throw new ArgumentException("El nombre de usuario ya está en uso.");
            }

            // Crear el nuevo usuario a partir del DTO y asignarle la suscripción Free
            var newUser = new Usuario
            {
                UserName = userDto.UserName,
                Password = userDto.Password,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName
            };

            // Guardar el usuario en el repositorio
            return _repository.AddUser(newUser);
        }

        public UsuarioDto GetUserById(int id)
        {
            var user = _repository.GetUserById(id);
            if (user == null)
            {
                return null;
            }
            return new UsuarioDto
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                Type = user.Type,
                SubscriptionStartDate = user.SubscriptionStartDate
            };
        }

        public UsuarioDto GetUserByUsername(string username)
        {
            var user = _repository.GetUserByUsername(username);
            if (user == null)
            {
                return null;
            }
            return new UsuarioDto
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                Type = user.Type,
                SubscriptionStartDate = user.SubscriptionStartDate
            };
        }

        public bool UpdateUser(UsuarioDto userDto)
        {
            var existingUser = _repository.GetUserByUsername(userDto.UserName);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado.");
            }

            existingUser.FirstName = userDto.FirstName;
            existingUser.LastName = userDto.LastName;
            existingUser.Password = userDto.Password;

            return _repository.UpdateUser(existingUser);
        }

        public bool DeleteUser(int userId) =>
            _repository.DeleteUser(userId);

        public bool UpdateUserSubscription(int userId, SuscripcionEnum newType) =>
            _repository.UpdateUserSubscription(userId, newType);
    }
}
