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
                throw new InvalidOperationException("Moneda no encontrada.");

            // 3) Calcular el resultado de la conversión
            decimal result = amount * (fromCurrencyEntity.IndiceConvertibilidad / toCurrencyEntity.IndiceConvertibilidad);

            // 4) Guardar la conversión en el repositorio y capturar el Id de la conversión creada
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

            // cuantas hizo en los ult 30 dias
            int usadasUltimos30 = _conversionRepository.CountUserConversionsSince(userId, desde);

            if (limite != int.MaxValue && usadasUltimos30 >= limite) // valida si NO es ilimitado y ya usó todas
            {
                // buscamos la conversión más vieja dentro de los últimos 30 días
                var oldest = _conversionRepository.GetOldestConversionDateSince(userId, desde);

                int diasRestantes = 0;

                if (oldest is DateTime fecha) 
                {
                    var fechaLiberacion = fecha.AddDays(30); // fecha en la que esa conversión "caduca"
                    var tiempoRestante = fechaLiberacion - ahora; // tiempo que falta para que se libere un intento
                    var dias = Math.Max(0, tiempoRestante.TotalDays); // si es negativo cambia a 0, sino puede mostrar decimales
                    diasRestantes = (int)Math.Ceiling(dias); // redondeamos para arriba porque si falta 1 día y 2 horas, queremos mostrar "2 días"
                }

                // Si llegamos acá es porque el usuario alcanzó el límite.403
                throw new InvalidOperationException(
                    $"Límite mensual alcanzado para plan {tipo}: {usadasUltimos30}/{limite}.\n" +
                    $"Volvés a tener intentos en aproximadamente {diasRestantes} día(s).");
            }

            // Este return solo se ejecuta si NO se alcanzó el límite.
            return usuario;
        }
    }
}
