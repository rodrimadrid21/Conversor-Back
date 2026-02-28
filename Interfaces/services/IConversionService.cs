using Conversor_Monedas_Api.DTOs;
using Conversor_Monedas_Api.Entities;
using System.Security.Claims;

namespace Conversor_Monedas_Api.Interfaces.services
{
    public interface IConversionService
    {
        List<ConversionDto> GetUserConversions(int UserId);

        ConversionDto ExecuteConversion(int userId, string fromCurrency, string toCurrency, decimal amount);



    }
}
