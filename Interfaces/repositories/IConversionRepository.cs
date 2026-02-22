using Conversor_Monedas_Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Conversor_Monedas_Api.Interfaces.repositories
{
    public interface IConversionRepository
    {
        //conversiones de un usuario por id
        List<Conversion> GetConversionsByUserId(int userId);

        // Registrar una nueva conversión
        int AddConversion(Conversion conversion);

        // 🔹 Nuevo método para el límite mensual
        int CountUserConversionsSince(int userId, DateTime fromDate);

        // 🔹 NUEVO: fecha de la conversión más vieja dentro de la ventana (desde fromDate)
        DateTime? GetOldestConversionDateSince(int userId, DateTime fromDate);
    }
}
