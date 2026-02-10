using Conversor_Monedas_Api.DTOs;
using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Enum;

namespace Conversor_Monedas_Api.Interfaces.services
{
    public interface ISuscripcionService
    {
        // 🔹 Catálogo completo de suscripciones
        Task<List<SuscripcionDto>> GetAllSubscriptionsAsync();

        // 🔹 Obtener una suscripción por tipo (Free / Trial / Pro)
        Task<SuscripcionDto?> GetSubscriptionByTypeAsync(SuscripcionEnum subscriptionType);

        // 🔹 Límite de conversiones (por ahora sigue por enum)
        int GetConversionLimit(SuscripcionEnum type);

        // 🔹 CRUD administrativo sobre la entidad Suscripcion
        Task CrearSuscripcionAsync(Suscripcion suscripcion);
        Task ActualizarSuscripcionAsync(Suscripcion suscripcion);
        Task EliminarSuscripcionAsync(int id);
    }
}
