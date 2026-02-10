using Conversor_Monedas_Api.DTOs;
using Conversor_Monedas_Api.Enum;
using Conversor_Monedas_Api.Interfaces.services;
using Conversor_Monedas_Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Conversor_Monedas_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuscripcionController : ControllerBase
    {
        private readonly ISuscripcionService _suscripcionService;

        public SuscripcionController(ISuscripcionService suscripcionService)
        {
            _suscripcionService = suscripcionService;
        }

        // Endpoint para obtener todas las suscripciones
        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var subscriptions = await _suscripcionService.GetAllSubscriptionsAsync();
            return Ok(subscriptions);
        }

        // Endpoint para obtener una suscripción por tipo
        [HttpGet("ByType/{type}")]
        public async Task<IActionResult> GetByType(SuscripcionEnum type)
        {
            var subscription = await _suscripcionService.GetSubscriptionByTypeAsync(type);
            if (subscription == null)
                return NotFound(new { Message = "Tipo de suscripción no encontrado." });

            return Ok(subscription);
        }

        // Endpoint opcional para obtener el límite de conversiones de un tipo específico de suscripción
        [HttpGet("Limit/{type}")]
        public IActionResult GetConversionLimit(SuscripcionEnum type)
        {
            var limit = _suscripcionService.GetConversionLimit(type);
            return Ok(new { Type = type.ToString(), ConversionLimit = limit });
        }
    }
}
