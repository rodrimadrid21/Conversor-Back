using Conversor_Monedas_Api.DTOs;
using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Enum;

namespace Conversor_Monedas_Api.Interfaces.services
{
    public interface ISuscripcionService
    {
        List<SuscripcionDto> GetAllSubscriptions();
        SuscripcionDto GetSubscriptionByType(SuscripcionEnum SubscriptionType);
        int GetConversionLimit(SuscripcionEnum type);
        Task CrearSuscripcionAsync(Suscripcion suscripcion);
        Task ActualizarSuscripcionAsync(Suscripcion suscripcion);
        Task EliminarSuscripcionAsync(int id);
    }
}
