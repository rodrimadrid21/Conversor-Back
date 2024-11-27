using Conversor_Monedas_Api.DTOs;
using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Enum;
using Conversor_Monedas_Api.Interfaces.repositories;
using Conversor_Monedas_Api.Interfaces.services;

namespace Conversor_Monedas_Api.Services
{
    public class ConversionService : IConversionService
    {
        private readonly IConversionRepository _conversionRepository;
        private readonly IMonedaRepository _currencyRepository;
        private readonly ISuscripcionService _subscriptionService;
        private readonly IUsuarioRepository _userRepository;

        public ConversionService(IConversionRepository conversionRepository, IMonedaRepository currencyRepository,
            ISuscripcionService subscriptionService, IUsuarioRepository userRepository)
        {
            _conversionRepository = conversionRepository;
            _currencyRepository = currencyRepository;
            _subscriptionService = subscriptionService;
            _userRepository = userRepository;
        }

        public ConversionDto ExecuteConversion(int userId, string fromCurrency, string toCurrency, decimal amount)
        {
            // Verificar si el usuario existe
            var user = _userRepository.GetUserById(userId);
            if (user == null)
                throw new Exception("Usuario no encontrado");

            // Determinar el límite de conversiones basado en el rol
            int roleLimit = user.Role switch
            {
                UsuarioEnum.Admin => int.MaxValue, // Ilimitado
                UsuarioEnum.user => 100, // Límite de 100 conversiones
                UsuarioEnum.Guest => 10, // Límite de 10 conversiones
                _ => throw new Exception("Rol no reconocido")
            };

            // Obtener las conversiones realizadas por el usuario
            var conversions = _conversionRepository.GetConversionsByUserId(userId);

            // Verificar si se alcanzó el límite
            if (conversions.Count >= roleLimit)
                throw new Exception("Has alcanzado el límite de conversiones permitido para tu rol.");


            // Obtener tasas de convertibilidad de las monedas
            var fromCurrencyEntity = _currencyRepository.GetCurrencyByCode(fromCurrency);
            var toCurrencyEntity = _currencyRepository.GetCurrencyByCode(toCurrency);

            if (fromCurrencyEntity == null || toCurrencyEntity == null)
                throw new Exception("Moneda no encontrada");

            // Calcular el resultado de la conversión
            decimal result = amount * (fromCurrencyEntity.IndiceConvertibilidad / toCurrencyEntity.IndiceConvertibilidad);

            // Guardar la conversión en el repositorio y capturar el Id de la conversión creada
            var conversion = new Conversion
            {
                Usuario = user,
                MonedaOrigen = fromCurrency,
                MonedaDestino = toCurrency,
                MontoOriginal = amount,
                MontoConvertido = result,
                FechaConversion = DateTime.UtcNow
            };
            int conversionId = _conversionRepository.AddConversion(conversion);

            // Devolver el DTO de conversión
            return new ConversionDto
            {
                ConversionId = conversionId,
                UsuarioId = userId,
                FromCurrency = fromCurrency,
                ToCurrency = toCurrency,
                FromCurrencySymbol = fromCurrencyEntity.Simbolo, // Agregar el símbolo de la moneda de origen
                ToCurrencySymbol = toCurrencyEntity.Simbolo, // Agregar el símbolo de la moneda de destino
                Amount = amount,
                Result = result,
                Date = conversion.FechaConversion
            };
        }

        public List<ConversionDto> GetUserConversions(int userId)
        {
            var conversions = _conversionRepository.GetConversionsByUserId(userId);
            return conversions.Select(c => new ConversionDto
            {
                ConversionId = c.ConversionId,
                UsuarioId = c.UsuarioId,
                FromCurrency = c.MonedaOrigen,
                ToCurrency = c.MonedaDestino,
                FromCurrencySymbol = _currencyRepository.GetCurrencyByCode(c.MonedaOrigen)?.Simbolo, // Obtener el símbolo de la moneda de origen
                ToCurrencySymbol = _currencyRepository.GetCurrencyByCode(c.MonedaDestino)?.Simbolo, // Obtener el símbolo de la moneda de destino
                Amount = c.MontoOriginal,
                Result = c.MontoConvertido,
                Date = c.FechaConversion
            }).ToList();
        }
    }
}
