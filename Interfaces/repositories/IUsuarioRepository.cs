using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Enum;

namespace Conversor_Monedas_Api.Interfaces.repositories
{
    public interface IUsuarioRepository
    {
        Usuario? Authenticate(string Name, string password);

        List<Usuario> GetAllUsers();

        Usuario GetUserById(int userId);

        Usuario GetUserByUsername(string Username);

        int AddUser(Usuario user);

        bool UpdateUser(Usuario user);

        bool DeleteUser(int userId);

        bool UpdateUserSubscription(int userId, SuscripcionEnum newType); 
    }
}
