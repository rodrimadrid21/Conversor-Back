using Conversor_Monedas_Api.Data;
using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Interfaces.repositories;
using Microsoft.EntityFrameworkCore;

namespace Conversor_Monedas_Api.Repositories
{
    public class MonedaRepository : IMonedaRepository
    {
        private readonly AppDbContext _context;

        public MonedaRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Moneda> GetAllCurrencies()
        {
            return _context.Moneda
                .Where(m => m.IsActive)
                .ToList();
        }

        public Moneda GetCurrencyById(int id)
        {
            return _context.Moneda
                .FirstOrDefault(c => c.Id == id && c.IsActive);
        }
        public Moneda GetCurrencyByCode(string code)
        {
            return _context.Moneda
                .FirstOrDefault(c => c.Codigo == code && c.IsActive);
        }

        public int AddCurrency(Moneda currency)
        {
            _context.Moneda.Add(currency);
            _context.SaveChanges();
            return currency.Id;
        }

        public bool UpdateCurrency(Moneda currency)
        {
            var existingCurrency = _context.Moneda.FirstOrDefault(c => c.Id == currency.Id && c.IsActive);
            if (existingCurrency == null)
            {
                return false;
            }

            // al registro existente en la bdd se le asigna el valor del objeto currency
            existingCurrency.Codigo = currency.Codigo;
            existingCurrency.Leyenda = currency.Leyenda;
            existingCurrency.Simbolo = currency.Simbolo;
            existingCurrency.IndiceConvertibilidad = currency.IndiceConvertibilidad;
            existingCurrency.IsActive = currency.IsActive;

            _context.Moneda.Update(existingCurrency);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteCurrency(int id)
        {
            var currency = _context.Moneda.FirstOrDefault(c => c.Id == id);
            if (currency != null)
            {
                currency.IsActive = false;        
                _context.Moneda.Update(currency); 
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
