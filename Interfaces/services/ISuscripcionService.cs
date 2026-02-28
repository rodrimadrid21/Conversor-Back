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

        //Límite (por enum)
        int GetConversionLimit(SuscripcionEnum type);

        void CrearSuscripcion(Suscripcion suscripcion);
        bool ActualizarSuscripcion(Suscripcion suscripcion);
        bool EliminarSuscripcion(int id);
    }
}