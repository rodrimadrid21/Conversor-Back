using Conversor_Monedas_Api.Data;
using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Interfaces.repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conversor_Monedas_Api.Repositories
{
    public class ConversionRepository : IConversionRepository
    {
        private readonly AppDbContext _context;

        public ConversionRepository(AppDbContext context)
        {
            _context = context;
        }

        // Obtener id por moneda
        public async Task<decimal> GetIdByCurrency(string codigoMoneda)
        {
            var moneda = await _context.Moneda
                                       .FirstOrDefaultAsync(m => m.Codigo == codigoMoneda);

            if (moneda == null)
                throw new KeyNotFoundException($"La moneda con el código '{codigoMoneda}' no fue encontrada.");

            return moneda.IndiceConvertibilidad;
        }

        // Obtener las conversiones de un usuario
        public async Task<IEnumerable<Conversion>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Conversion
                .Where(c => c.UsuarioId == usuarioId) // Ahora compara con int
                .ToListAsync();
        }


        // Obtener una conversión por su ID
        public List<Conversion> GetConversionsByUserId(int userId)
        {
            return _context.Conversion.Where(c => c.UsuarioId == userId).ToList();
        }

        // Registrar una nueva conversión
        public int AddConversion(Conversion conversion)
        {
            _context.Conversion.Add(conversion);
            _context.SaveChanges();
            return conversion.ConversionId;
        }
    }
}

