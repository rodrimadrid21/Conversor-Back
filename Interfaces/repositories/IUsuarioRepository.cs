using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Enum;

namespace Conversor_Monedas_Api.Interfaces.repositories
{
    public interface IUsuarioRepository
    {
        // Autenticacion del usuario
        Usuario? Authenticate(string Name, string password);

        // Obtener todos los usuarios
        List<Usuario> GetAllUsers();

        // Obtener un usuario por su ID
        Usuario GetUserById(int userId);

        // Obtener un usuario por su usuario
        Usuario GetUserByUsername(string Username);

        // Crear un nuevo usuario
        int AddUser(Usuario user);

        // Actualizar la información de un usuario
        bool UpdateUser(Usuario user);

        // Eliminar un usuario
        bool DeleteUser(int userId);

        // Actualizar suscripcion de usuario
        bool UpdateUserSubscription(int userId, SuscripcionEnum newType); 
    }
}
