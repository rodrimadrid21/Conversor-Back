using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Enum;
using System.Collections.Generic;

namespace Conversor_Monedas_Api.Interfaces.repositories
{
    public interface ISuscripcionRepository
    {
        // Obtener todas las suscripciones
        List<Suscripcion> GetAllSubscriptions();

        // Obtener una suscripción por tipo (Free/Trial/Pro)
        Suscripcion? GetSubscriptionByType(SuscripcionEnum type);

        // Obtener una suscripción por ID
        Suscripcion? GetById(int id);

        // Crear una nueva suscripción
        void Add(Suscripcion subscription);

        // Actualizar los detalles de la suscripción
        bool Update(Suscripcion subscription);   // ← cambiado

        // Eliminar una suscripción
        bool Delete(int subscriptionId);         // ← cambiado
    }
}