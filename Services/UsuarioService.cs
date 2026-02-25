using Conversor_Monedas_Api.DTOs;
using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Enum;
using Conversor_Monedas_Api.Interfaces.repositories;
using Conversor_Monedas_Api.Interfaces.services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;
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
            return _repository.Authenticate(credentials.UserName, credentials.Password);
        }
        public string Authenticate(AuthenticationDto credentials)
        {
            Usuario user = ValidateUser(credentials);
            if (user == null)
            {
                return null;
            }

            return GenerateJwtToken(user); //si esta validado genera el token y autentica al user
        }
        public string GenerateJwtToken(Usuario user)
        {
            // 1) Definir la clave secreta y el algoritmo de firma para el token
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );

            // 2) Crear las credenciales de firma utilizando la clave secreta y el algoritmo HMAC-SHA256
            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            // 3.1) inicialización de colección de las reclamaciones (claims) del token - crea los datos del user (userId, nameS, subscriptiontype)
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),     // "sub.ject => userId"
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName ?? ""),     
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName ?? ""),    
                new Claim("subscriptionType", user.Type.ToString()),                // reclamación personalizada para incluir el tipo de suscripción.
            };
            // 3.2) contstruccion del token. 
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,                             //1. se agrega las claims personalizada
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials//2. y se agrega las credenciales de firma para que el token quede firmado con la clave secreta y el algoritmo HMAC-SHA256.
            );

            // 4) Convertir el token a string para enviarlo al cliente. 
            return new JwtSecurityTokenHandler().WriteToken(token); // El handler se encarga de serializar el token con su header, payload y firma.
        }

        // obtiene el id del usuario auth para activar el plan ⬇
        public int GetUserIdFromContext(ClaimsPrincipal user)
        {
            var claim = user?.FindFirst(JwtRegisteredClaimNames.Sub);

            if (claim == null)
                throw new UnauthorizedAccessException("Usuario no autenticado.");

            return int.Parse(claim.Value); // Si existe, lo convierto a int y lo devuelvo.
        }

        public List<UsuarioDto> GetAllUsers()
        {
            var users = _repository.GetAllUsers();
            return users.Select(u => new UsuarioDto // mapea cada Usuario a UsuarioDto - para cada usuario "u", se crea un dto
            {
                UserId = u.UserId,
                UserName = u.UserName,
                Password = u.Password,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Type = u.Type,
                SubscriptionStartDate = u.SubscriptionStartDate
            }).ToList();
        }

        public int RegisterUser(UsuarioDto userDto)
        {
            // Verif si el usuario existe
            var user = _repository.GetUserByUsername(userDto.UserName);
            if (user != null)
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
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
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
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Type = user.Type,
                SubscriptionStartDate = user.SubscriptionStartDate
            };
        }

        public bool UpdateUser(UsuarioDto userDto)
        {
            var user = _repository.GetUserByUsername(userDto.UserName);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado.");
            }

            // actualizar los campos del usuario con los datos del DTO
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Password = userDto.Password;

            return _repository.UpdateUser(user);
        }

        public bool DeleteUser(int userId) =>
            _repository.DeleteUser(userId);

        public bool UpdateUserSubscription(int userId, SuscripcionEnum newType) =>
            _repository.UpdateUserSubscription(userId, newType);
    }
}
