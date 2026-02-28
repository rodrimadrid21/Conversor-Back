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

        public List<Conversion> GetConversionsByUserId(int userId)
        {
            return _context.Conversion
                .Where(c => c.UsuarioId == userId)
                .OrderByDescending(c => c.FechaConversion)
                .ToList();
        }

        public int AddConversion(Conversion conversion)
        {
            _context.Conversion.Add(conversion);
            _context.SaveChanges();
            return conversion.ConversionId;
        }

        public int CountUserConversionsSince(int userId, DateTime fromDate)
        {
            return _context.Conversion
                .Count(c => c.UsuarioId == userId && c.FechaConversion >= fromDate);
        }

        // la mas vieja
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