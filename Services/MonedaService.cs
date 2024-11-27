using Conversor_Monedas_Api.DTOs;
using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Interfaces.repositories;
using Conversor_Monedas_Api.Interfaces.services;

namespace Conversor_Monedas_Api.Services
{
    public class MonedaService : IMonedaService
    {
        private readonly IMonedaRepository _currencyRepository;

        public MonedaService(IMonedaRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public List<MonedaDto> GetAllCurrencies()
        {
            var currencies = _currencyRepository.GetAllCurrencies();
            return currencies.Select(c => new MonedaDto
            {
                CurrencyId = c.Id,
                Code = c.Codigo,
                Legend = c.Leyenda,
                Symbol = c.Simbolo,
                ConvertibilityIndex = c.IndiceConvertibilidad
            }).ToList();
        }

        public MonedaDto GetCurrencyById(int id)
        {
            var currency = _currencyRepository.GetCurrencyById(id);
            if (currency == null)
            {
                return null;
            }
            return new MonedaDto
            {
                CurrencyId = currency.Id,
                Code = currency.Codigo,
                Legend = currency.Leyenda,
                Symbol = currency.Simbolo,
                ConvertibilityIndex = currency.IndiceConvertibilidad
            };
        }

        public int AddCurrency(MonedaDto currencyDto)
        {
            var currency = new Moneda
            {
                Codigo = currencyDto.Code,
                Leyenda = currencyDto.Legend,
                Simbolo = currencyDto.Symbol,
                IndiceConvertibilidad = currencyDto.ConvertibilityIndex
            };
            return _currencyRepository.AddCurrency(currency);
        }

        public bool UpdateCurrency(int id, MonedaDto currencyDto)
        {
            var existingCurrency = _currencyRepository.GetCurrencyById(id);
            if (existingCurrency == null)
            {
                throw new KeyNotFoundException($"No se encontró una moneda con el ID {id}");
            }

            // Actualizamos solo los campos necesarios
            existingCurrency.Codigo = currencyDto.Code;
            existingCurrency.Leyenda = currencyDto.Legend;
            existingCurrency.Simbolo = currencyDto.Symbol;
            existingCurrency.IndiceConvertibilidad = currencyDto.ConvertibilityIndex;

            return _currencyRepository.UpdateCurrency(existingCurrency);
        }

        public bool DeleteCurrency(int id)
        {
            return _currencyRepository.DeleteCurrency(id);
        }
    }
}
