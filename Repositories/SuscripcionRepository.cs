using Conversor_Monedas_Api.Data;
using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Enum;
using Conversor_Monedas_Api.Interfaces.repositories;
using System.Collections.Generic;
using System.Linq;

namespace Conversor_Monedas_Api.Repositories
{
    public class SuscripcionRepository : ISuscripcionRepository
    {
        private readonly AppDbContext _context;

        public SuscripcionRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Suscripcion> GetAllSubscriptions()
        {
            return _context.Suscripcion.ToList();
        }

        public Suscripcion? GetSubscriptionByType(SuscripcionEnum type)
        {
            return _context.Suscripcion.FirstOrDefault(s => s.Tipo == type);
        }

        public Suscripcion? GetById(int id)
        {
            return _context.Suscripcion.Find(id);
        }

        public void Add(Suscripcion subscription)
        {
            _context.Suscripcion.Add(subscription);
            _context.SaveChanges();
        }

        public bool Update(Suscripcion subscription)
        {
            var sub = _context.Suscripcion.FirstOrDefault(s => s.Id == subscription.Id);
            if (sub == null)
                return false;

            // al registro existente en la bdd se le asigna el valor del objeto suscription
            sub.Tipo = subscription.Tipo;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var suscripcion = GetById(id);
            if (suscripcion == null)
                return false;

            _context.Suscripcion.Remove(suscripcion);
            _context.SaveChanges();
            return true;
        }
    }
}