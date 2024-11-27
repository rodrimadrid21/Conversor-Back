using Conversor_Monedas_Api.DTOs;
using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Enum;
using Conversor_Monedas_Api.Interfaces.repositories;
using Conversor_Monedas_Api.Interfaces.services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Conversor_Monedas_Api.Services
{
    public class SuscripcionService : ISuscripcionService
    {
        private readonly ISuscripcionRepository _subscriptionRepository;

        public SuscripcionService(ISuscripcionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public SuscripcionDto GetSubscriptionByType(SuscripcionEnum type)
        {
            var subscription = _subscriptionRepository.GetSubscriptionByType(type);
            if (subscription == null) return null;

            return new SuscripcionDto
            {
                Id = subscription.Id,
                Type = subscription.Tipo,
                ConversionLimit = GetConversionLimit(subscription.Tipo),
                MonthlyReset = subscription.MaximoConversiones
            };
        }

        public int GetConversionLimit(SuscripcionEnum type)
        {
            return type switch//
            {
                SuscripcionEnum.Free => 10,
                SuscripcionEnum.Trial => 100,
                SuscripcionEnum.Pro => int.MaxValue,
            };
        }

        public List<SuscripcionDto> GetAllSubscriptions()
        {
            var subscriptions = _subscriptionRepository.GetAllSubscriptions();

            return subscriptions.Select(subscription => new SuscripcionDto
            {
                Id = subscription.Id,
                Type = subscription.Tipo,
                ConversionLimit = GetConversionLimit(subscription.Tipo),
                MonthlyReset = subscription.MaximoConversiones
            }).ToList();
        }

        public async Task CrearSuscripcionAsync(Suscripcion suscripcion)
        {
            await _subscriptionRepository.AddAsync(suscripcion);
        }

        public async Task ActualizarSuscripcionAsync(Suscripcion suscripcion)
        {
            await _subscriptionRepository.UpdateAsync(suscripcion);
        }

        public async Task EliminarSuscripcionAsync(int id)
        {
            await _subscriptionRepository.DeleteAsync(id);
        }
    }
}
