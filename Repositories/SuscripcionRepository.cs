using Conversor_Monedas_Api.Data;
using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Enum;
using Conversor_Monedas_Api.Interfaces.repositories;
using Microsoft.EntityFrameworkCore;

namespace Conversor_Monedas_Api.Repositories
{
    public class SuscripcionRepository : ISuscripcionRepository
    {
        private readonly AppDbContext _context;

        public SuscripcionRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Suscripcion> GetAllSubscriptions()
        {
            return _context.Suscripcion.ToList();
        }

        public Suscripcion GetSubscriptionByType(SuscripcionEnum type)
        {
            return _context.Suscripcion.FirstOrDefault(s => s.Tipo == type);
        }

        public async Task<Suscripcion> GetByIdAsync(int id)
        {
            return await _context.Suscripcion.FindAsync(id);
        }

        public async Task AddAsync(Suscripcion suscripcion)
        {
            await _context.Suscripcion.AddAsync(suscripcion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Suscripcion suscripcion)
        {
            _context.Suscripcion.Update(suscripcion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var suscripcion = await GetByIdAsync(id);
            if (suscripcion != null)
            {
                _context.Suscripcion.Remove(suscripcion);
                await _context.SaveChangesAsync();
            }
        }
    }
}
