using Conversor_Monedas_Api.DTOs;
using Conversor_Monedas_Api.Entities;
using System.Security.Claims;

namespace Conversor_Monedas_Api.Interfaces.services
{
    public interface IConversionService
    {
        //conseguir las conversiones de un usuario por su id
        List<ConversionDto> GetUserConversions(int UserId);

        //public int AddConversion(Conversion conversion);
        ConversionDto ExecuteConversion(int userId, string fromCurrency, string toCurrency, decimal amount);



    }
}
