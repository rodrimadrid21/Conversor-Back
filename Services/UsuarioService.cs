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
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("sub", user.UserId.ToString()),
                new Claim("given_name", user.FirstName),
                new Claim("family_name", user.LastName),
                new Claim("role", user.Role.ToString())
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
            var userIdClaim = user?.FindFirst("sub");
            if (userIdClaim != null)
            {
                return int.Parse(userIdClaim.Value);
            }
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
                Role = u.Role,
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
                LastName = userDto.LastName,
                Role = UsuarioEnum.user,
                Type = SuscripcionEnum.Free,
                SubscriptionStartDate = DateTime.UtcNow,
                IsActive = true
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
            existingUser.Role = userDto.Role;
            existingUser.Type = userDto.Type;

            return _repository.UpdateUser(existingUser);
        }

        public bool DeleteUser(int userId) =>
            _repository.DeleteUser(userId);

        public bool UpdateUserSubscription(int userId, SuscripcionEnum newType) =>
            _repository.UpdateUserSubscription(userId, newType);
    }
}
