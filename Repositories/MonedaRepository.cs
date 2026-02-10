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

        // 🔹 Devolver solo monedas activas
        public List<Moneda> GetAllCurrencies()
        {
            return _context.Moneda
                .Where(m => m.IsActive)
                .ToList();
        }

        // 🔹 Buscar por ID solo si está activa
        public Moneda GetCurrencyById(int id)
        {
            return _context.Moneda
                .FirstOrDefault(c => c.Id == id && c.IsActive);
        }

        // 🔹 Buscar por código solo si está activa
        public Moneda GetCurrencyByCode(string code)
        {
            return _context.Moneda
                .FirstOrDefault(c => c.Codigo == code && c.IsActive);
        }

        public int AddCurrency(Moneda currency)
        {
            currency.IsActive = true; // Asegurar que entra como activa
            _context.Moneda.Add(currency);
            _context.SaveChanges();
            return currency.Id;
        }

        public bool UpdateCurrency(Moneda currency)
        {
            var existingCurrency = _context.Moneda.FirstOrDefault(m => m.Id == currency.Id);
            if (existingCurrency == null)
            {
                return false; // No existe la moneda
            }

            existingCurrency.Codigo = currency.Codigo;
            existingCurrency.Leyenda = currency.Leyenda;
            existingCurrency.Simbolo = currency.Simbolo;
            existingCurrency.IndiceConvertibilidad = currency.IndiceConvertibilidad;
            existingCurrency.IsActive = currency.IsActive;

            _context.Moneda.Update(existingCurrency);
            _context.SaveChanges();
            return true;
        }

        // 🔥 BAJA LÓGICA — aquí está el FIX opcional
        public bool DeleteCurrency(int id)
        {
            var currency = _context.Moneda.FirstOrDefault(c => c.Id == id);
            if (currency != null)
            {
                currency.IsActive = false;        // ⚡ en vez de eliminarla
                _context.Moneda.Update(currency); // la marcamos como inactiva
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
