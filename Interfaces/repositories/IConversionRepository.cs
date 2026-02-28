using Conversor_Monedas_Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Conversor_Monedas_Api.Interfaces.repositories
{
    public interface IConversionRepository
    {
        List<Conversion> GetConversionsByUserId(int userId);

        int AddConversion(Conversion conversion);

        // desde
        int CountUserConversionsSince(int userId, DateTime fromDate);

        // get conversion mas vieja
        DateTime? GetOldestConversionDateSince(int userId, DateTime fromDate);
    }
}
