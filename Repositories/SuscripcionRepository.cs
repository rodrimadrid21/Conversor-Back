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

        // Obtener todas las suscripciones (catálogo)
        public async Task<List<Suscripcion>> GetAllSubscriptionsAsync()
        {
            return await _context.Suscripcion.ToListAsync();
        }

        // Obtener una suscripción por tipo (Free / Trial / Pro)
        public async Task<Suscripcion?> GetSubscriptionByTypeAsync(SuscripcionEnum type)
        {
            return await _context.Suscripcion
                .FirstOrDefaultAsync(s => s.Tipo == type);
        }

        // Obtener suscripción por Id
        public async Task<Suscripcion?> GetByIdAsync(int id)
        {
            return await _context.Suscripcion.FindAsync(id);
        }

        // Crear una nueva suscripción (normalmente solo para seed / admin)
        public async Task AddAsync(Suscripcion suscripcion)
        {
            await _context.Suscripcion.AddAsync(suscripcion);
            await _context.SaveChangesAsync();
        }

        // Actualizar una suscripción (ej: cambiar límites)
        public async Task UpdateAsync(Suscripcion suscripcion)
        {
            _context.Suscripcion.Update(suscripcion);
            await _context.SaveChangesAsync();
        }

        // Eliminar una suscripción (raro en la práctica, pero queda)
        public async Task DeleteAsync(int id)
        {
            var suscripcion = await GetByIdAsync(id);
            if (suscripcion == null)
                return;

            _context.Suscripcion.Remove(suscripcion);
            await _context.SaveChangesAsync();
        }
    }
}
