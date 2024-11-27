using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Enum;

namespace Conversor_Monedas_Api.Interfaces.repositories
{
    public interface ISuscripcionRepository
    {
        // Obtener todas las suscripciones
        List<Suscripcion> GetAllSubscriptions(); 

        // Obtener una suscripción por su ID
        Suscripcion? GetSubscriptionByType(SuscripcionEnum type); 

        // Crear una nueva suscripción
        Task AddAsync(Suscripcion subscription); 

        // Actualizar los detalles de la suscripción
        Task UpdateAsync(Suscripcion subscription); 

        // Eliminar una suscripción
        Task DeleteAsync(int subscriptionId); 
    }
}
