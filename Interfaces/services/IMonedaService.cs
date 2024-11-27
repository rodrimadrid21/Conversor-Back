using Conversor_Monedas_Api.DTOs;
using Conversor_Monedas_Api.Entities;

namespace Conversor_Monedas_Api.Interfaces.services
{
    public interface IMonedaService
    {
        List<MonedaDto> GetAllCurrencies();
        MonedaDto GetCurrencyById(int currencyId);
        int AddCurrency(MonedaDto currency);
        bool UpdateCurrency(int currencyID, MonedaDto currency);
        bool DeleteCurrency(int id);
    }
}
