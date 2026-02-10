using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Enum;

namespace Conversor_Monedas_Api.Interfaces.repositories
{
    public interface ISuscripcionRepository
    {
        /// Obtener todas las suscripciones
        Task<List<Suscripcion>> GetAllSubscriptionsAsync();

        // Obtener una suscripción por tipo (Free/Trial/Pro)
        Task<Suscripcion?> GetSubscriptionByTypeAsync(SuscripcionEnum type);

        // Obtener una suscripción por ID
        Task<Suscripcion?> GetByIdAsync(int id);

        // Crear una nueva suscripción
        Task AddAsync(Suscripcion subscription); 

        // Actualizar los detalles de la suscripción
        Task UpdateAsync(Suscripcion subscription); 

        // Eliminar una suscripción
        Task DeleteAsync(int subscriptionId); 
    }
}
