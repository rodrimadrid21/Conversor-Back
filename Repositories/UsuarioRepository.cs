using Conversor_Monedas_Api.Data;
using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Enum;
using Conversor_Monedas_Api.Interfaces.repositories;
using Microsoft.EntityFrameworkCore;

namespace Conversor_Monedas_Api.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public Usuario? Authenticate(string username, string password)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);
        }

        public List<Usuario> GetAllUsers()
        {
            return _context.Users.Where(u=>u.IsActive).ToList();
        }

        public Usuario GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == id);
        }

        public Usuario GetUserByUsername(string Username)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == Username);
        }

        public int AddUser(Usuario user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }

        public bool UpdateUser(Usuario user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.UserId == user.UserId);
            if (existingUser != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Password = user.Password;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;

                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                user.IsActive = false;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateUserSubscription(int userId, SuscripcionEnum newType)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return false;
            }
            user.Type = newType;
            user.SubscriptionStartDate = DateTime.UtcNow; 

            _context.SaveChanges();
            return true;

        }
    }
}
