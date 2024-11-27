using Conversor_Monedas_Api.Entities;

namespace Conversor_Monedas_Api.Interfaces.repositories
{
    public interface IMonedaRepository
    {
        // Obtener todas las monedas
        List<Moneda> GetAllCurrencies();

        // Obtener una moneda por su ID
        Moneda GetCurrencyById(int currencyId);

        // Obtener una moneda por su codigo
        Moneda GetCurrencyByCode(string code);

        // Crear una nueva moneda
        int AddCurrency(Moneda currency);

        // Actualizar los detalles de una moneda
        bool UpdateCurrency(Moneda currency);

        // Eliminar una moneda
        bool DeleteCurrency(int currencyId); 
    }
}
