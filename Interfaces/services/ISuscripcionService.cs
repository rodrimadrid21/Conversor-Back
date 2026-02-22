using Conversor_Monedas_Api.DTOs;
using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Enum;
using System.Collections.Generic;

namespace Conversor_Monedas_Api.Interfaces.services
{
    public interface ISuscripcionService
    {
        List<SuscripcionDto> GetAllSubscriptions();

        SuscripcionDto? GetSubscriptionByType(SuscripcionEnum subscriptionType);

        // 🔹 Límite de conversiones (por enum)
        int GetConversionLimit(SuscripcionEnum type);

        // 🔹 CRUD administrativo sobre la entidad Suscripcion
        void CrearSuscripcion(Suscripcion suscripcion);

        // ✅ ahora devuelven bool para saber si existía
        bool ActualizarSuscripcion(Suscripcion suscripcion);
        bool EliminarSuscripcion(int id);
    }
}