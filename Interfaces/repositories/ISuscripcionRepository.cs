using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Enum;
using System.Collections.Generic;

namespace Conversor_Monedas_Api.Interfaces.repositories
{
    public interface ISuscripcionRepository
    {
        List<Suscripcion> GetAllSubscriptions();

        Suscripcion? GetSubscriptionByType(SuscripcionEnum type);

        Suscripcion? GetById(int id);

        void Add(Suscripcion subscription);

        bool Update(Suscripcion subscription);

        bool Delete(int subscriptionId);
    }
}