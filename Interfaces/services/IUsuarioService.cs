using Conversor_Monedas_Api.DTOs;
using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Enum;
using System.Security.Claims;

namespace Conversor_Monedas_Api.Interfaces.services
{
    public interface IUsuarioService
    {
        string Authenticate(AuthenticationDto credentials);
        Usuario ValidateUser(AuthenticationDto credentials);
        string GenerateJwtToken(Usuario user);
        int GetUserIdFromContext(ClaimsPrincipal user);
        int RegisterUser(UsuarioDto userDto);
        bool UpdateUser(UsuarioDto userDto);
        bool DeleteUser(int userId);
        UsuarioDto GetUserById(int userId);
        UsuarioDto GetUserByUsername(string username);
        List<UsuarioDto> GetAllUsers();
        bool UpdateUserSubscription(int userId, SuscripcionEnum newType);
    }
}
