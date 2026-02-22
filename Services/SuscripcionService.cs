using Conversor_Monedas_Api.DTOs;
using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Enum;
using Conversor_Monedas_Api.Interfaces.repositories;
using Conversor_Monedas_Api.Interfaces.services;
using System.Collections.Generic;
using System.Linq;

namespace Conversor_Monedas_Api.Services
{
    public class SuscripcionService : ISuscripcionService
    {
        private readonly ISuscripcionRepository _subscriptionRepository;

        public SuscripcionService(ISuscripcionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public SuscripcionDto? GetSubscriptionByType(SuscripcionEnum type)
        {
            var subscription = _subscriptionRepository.GetSubscriptionByType(type);
            if (subscription == null) return null;

            return new SuscripcionDto
            {
                Id = subscription.Id,
                Type = subscription.Tipo,
                ConversionLimit = GetConversionLimit(subscription.Tipo),
            };
        }

        public int GetConversionLimit(SuscripcionEnum type)
        {
            switch (type)
            {
                case SuscripcionEnum.Free:
                    return 2;

                case SuscripcionEnum.Trial:
                    return 100;

                case SuscripcionEnum.Pro:
                    return int.MaxValue;

                default:
                    return 0;
            }
        }

        public List<SuscripcionDto> GetAllSubscriptions()
        {
            var subscriptions = _subscriptionRepository.GetAllSubscriptions();

            return subscriptions.Select(subscription => new SuscripcionDto
            {
                Id = subscription.Id,
                Type = subscription.Tipo,
                ConversionLimit = GetConversionLimit(subscription.Tipo)
            }).ToList();
        }

        public void CrearSuscripcion(Suscripcion suscripcion)
        {
            _subscriptionRepository.Add(suscripcion);
        }

        public bool ActualizarSuscripcion(Suscripcion suscripcion)
        {
            return _subscriptionRepository.Update(suscripcion);
        }

        public bool EliminarSuscripcion(int id)
        {
            return _subscriptionRepository.Delete(id);
        }
    }
}