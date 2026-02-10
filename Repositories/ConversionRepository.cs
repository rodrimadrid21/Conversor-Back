using Conversor_Monedas_Api.Data;
using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Interfaces.repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Conversor_Monedas_Api.Repositories
{
    public class ConversionRepository : IConversionRepository
    {
        private readonly AppDbContext _context;

        public ConversionRepository(AppDbContext context)
        {
            _context = context;
        }

        // Obtener índice por moneda (nombre raro, pero lo dejo como lo tenés)
        public async Task<decimal> GetIdByCurrency(string codeCurrency)
        {
            var moneda = await _context.Moneda
                .FirstOrDefaultAsync(m => m.Codigo == codeCurrency);

            if (moneda == null)
                throw new KeyNotFoundException($"La moneda con el código '{codeCurrency}' no fue encontrada.");

            return moneda.IndiceConvertibilidad;
        }

        // Obtener las conversiones de un usuario
        public List<Conversion> GetConversionsByUserId(int userId)
        {
            return _context.Conversion
                .Where(c => c.UsuarioId == userId)
                .ToList();
        }

        // Registrar una nueva conversión
        public int AddConversion(Conversion conversion)
        {
            _context.Conversion.Add(conversion);
            _context.SaveChanges();
            return conversion.ConversionId;
        }

        // Límite mensual: cuenta conversiones desde una fecha
        public int CountUserConversionsSince(int userId, DateTime fromDate)
        {
            return _context.Conversion
                .Count(c => c.UsuarioId == userId && c.FechaConversion >= fromDate);
        }

        // ✅ NUEVO: fecha más vieja dentro de la ventana (para calcular días restantes)
        public DateTime? GetOldestConversionDateSince(int userId, DateTime fromDate)
        {
            return _context.Conversion
                .Where(c => c.UsuarioId == userId && c.FechaConversion >= fromDate)
                .OrderBy(c => c.FechaConversion)
                .Select(c => (DateTime?)c.FechaConversion)
                .FirstOrDefault();
        }
    }
}
