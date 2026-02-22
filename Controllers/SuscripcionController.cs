using Conversor_Monedas_Api.Enum;
using Conversor_Monedas_Api.Interfaces.services;
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

        [HttpGet("All")]
        public IActionResult GetAll()
        {
            var subscriptions = _suscripcionService.GetAllSubscriptions();
            return Ok(subscriptions);
        }

        [HttpGet("ByType/{type}")]
        public IActionResult GetByType(SuscripcionEnum type)
        {
            var subscription = _suscripcionService.GetSubscriptionByType(type);

            if (subscription == null)
                return NotFound(new { Message = "Tipo de suscripción no encontrado." });

            return Ok(subscription);
        }

    }
}