using Conversor_Monedas_Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Conversor_Monedas_Api.Interfaces.repositories
{
    public interface IConversionRepository
    {
        //conversiones de un usuario por id
        List<Conversion> GetConversionsByUserId(int userId);

        //obtener ind por moneda
        Task<decimal> GetIdByCurrency(string codeCurrency);

        // Registrar una nueva conversión
        int AddConversion(Conversion conversion); 
    }
}
