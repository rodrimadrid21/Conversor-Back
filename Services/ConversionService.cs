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

        public ConversionService(
            IConversionRepository conversionRepository,
            IMonedaRepository currencyRepository,
            ISuscripcionService subscriptionService,
            IUsuarioRepository userRepository)
        {
            _conversionRepository = conversionRepository;
            _currencyRepository = currencyRepository;
            _subscriptionService = subscriptionService;
            _userRepository = userRepository;
        }

        public ConversionDto ExecuteConversion(int userId, string fromCurrency, string toCurrency, decimal amount)
        {
            // valida limite
            var user = ValidarLimiteSuscripcion(userId);

            // get tasas de convertibilidad de las monedas
            var fromCurrencyEntity = _currencyRepository.GetCurrencyByCode(fromCurrency);
            var toCurrencyEntity = _currencyRepository.GetCurrencyByCode(toCurrency);

            if (fromCurrencyEntity == null || toCurrencyEntity == null)
                throw new InvalidOperationException("Moneda no encontrada.");

            decimal result = amount * (fromCurrencyEntity.IndiceConvertibilidad / toCurrencyEntity.IndiceConvertibilidad);

            // guardamos y creamos la entidad
            var conversion = new Conversion
            {
                Usuario = user,
                UsuarioId = user.UserId,
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
                FromCurrencySymbol = fromCurrencyEntity.Simbolo,
                ToCurrencySymbol = toCurrencyEntity.Simbolo,
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
                Amount = c.MontoOriginal,
                Result = c.MontoConvertido,
                Date = c.FechaConversion
            }).ToList();
        }
        private Usuario ValidarLimiteSuscripcion(int userId)
        {
            var usuario = _userRepository.GetUserById(userId);

            if (usuario == null || !usuario.IsActive)
                throw new UnauthorizedAccessException("Usuario no válido o inactivo.");

            var tipo = usuario.Type;
            int limite = _subscriptionService.GetConversionLimit(tipo);

            var ahora = DateTime.UtcNow;
            var desde = ahora.AddDays(-30); // los ultimos 30 días

            // cuantas
            int usadasUltimos30 = _conversionRepository.CountUserConversionsSince(userId, desde);

            if (limite != int.MaxValue && usadasUltimos30 >= limite)
            {
                // la mas vieja
                var oldest = _conversionRepository.GetOldestConversionDateSince(userId, desde);

                int diasRestantes = 0;

                if (oldest is DateTime fecha)
                {
                    var fechaLiberacion = fecha.AddDays(30); // cuando "caduca"
                    var tiempoRestante = fechaLiberacion - ahora;
                    var dias = Math.Max(0, tiempoRestante.TotalDays);
                    diasRestantes = (int)Math.Ceiling(dias); // redondeamos para arriba
                }

                //403
                throw new InvalidOperationException(
                    $"Límite mensual alcanzado para plan {tipo}: {usadasUltimos30}/{limite}.\n" +
                    $"Volvés a tener intentos en aproximadamente {diasRestantes} día(s).");
            }

            // si NO se alcanzó el límite o es pro
            return usuario;
        }
    }
}
