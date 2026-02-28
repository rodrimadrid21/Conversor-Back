using Conversor_Monedas_Api.Entities;

namespace Conversor_Monedas_Api.Interfaces.repositories
{
    public interface IMonedaRepository
    {
        List<Moneda> GetAllCurrencies();

        Moneda GetCurrencyById(int currencyId);

        Moneda GetCurrencyByCode(string code);

        int AddCurrency(Moneda currency);

        bool UpdateCurrency(Moneda currency);

        bool DeleteCurrency(int currencyId); 
    }
}
