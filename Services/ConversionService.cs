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
            // 1) Valida suscripción y trae el usuario
            var user = ValidarLimiteSuscripcion(userId);

            // 2) Obtener tasas de convertibilidad de las monedas
            var fromCurrencyEntity = _currencyRepository.GetCurrencyByCode(fromCurrency);
            var toCurrencyEntity = _currencyRepository.GetCurrencyByCode(toCurrency);

            if (fromCurrencyEntity == null || toCurrencyEntity == null)
                throw new Exception("Moneda no encontrada");

            // 3) Calcular el resultado de la conversión
            decimal result = amount * (fromCurrencyEntity.IndiceConvertibilidad / toCurrencyEntity.IndiceConvertibilidad);

            // 4) Guardar la conversión en el repositorio y capturar el Id de la conversión creada
            var conversion = new Conversion
            {
                Usuario = user,
                UsuarioId = user.UserId,       // si tu entidad tiene esta FK
                MonedaOrigen = fromCurrency,
                MonedaDestino = toCurrency,
                MontoOriginal = amount,
                MontoConvertido = result,
                FechaConversion = DateTime.UtcNow
            };
            int conversionId = _conversionRepository.AddConversion(conversion);

            // 5) Devolver el DTO de conversión
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
                FromCurrencySymbol = _currencyRepository.GetCurrencyByCode(c.MonedaOrigen)?.Simbolo,
                ToCurrencySymbol = _currencyRepository.GetCurrencyByCode(c.MonedaDestino)?.Simbolo,
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
            var desde = ahora.AddDays(-30);

            int usadasUltimos30 = _conversionRepository.CountUserConversionsSince(userId, desde);

            if (limite != int.MaxValue && usadasUltimos30 >= limite)
            {
                // buscamos la conversión más vieja dentro de los últimos 30 días
                var oldest = _conversionRepository.GetOldestConversionDateSince(userId, desde);

                int diasRestantes = 0;
                if (oldest.HasValue)
                {
                    var resetAt = oldest.Value.AddDays(30);
                    var remaining = resetAt - ahora;

                    // ceil: si faltan 0.2 días, mostramos 1 día
                    diasRestantes = (int)Math.Ceiling(Math.Max(0, remaining.TotalDays));
                }

                throw new InvalidOperationException(
                    $"Límite mensual alcanzado para plan {tipo}: {usadasUltimos30}/{limite}.\n" +
                    $"Volvés a tener intentos en aproximadamente {diasRestantes} día(s).");
            }

            return usuario;
        }
    }
}
