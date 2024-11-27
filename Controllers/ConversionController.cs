using Conversor_Monedas_Api.DTOs;
using Conversor_Monedas_Api.Enum;
using Conversor_Monedas_Api.Interfaces.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Conversor_Monedas_Api.Controllers
{
    [Authorize] // Esto asegura que todos los métodos del controlador requieren autorización
    [Route("api/[controller]")]
    [ApiController]
    public class ConversionController : ControllerBase
    {
        private readonly IConversionService _conversionService;
        private readonly IUsuarioService _usuarioService; // Servicio para obtener el UserId


        public ConversionController(IConversionService conversionService, IUsuarioService usuarioService)
        {
            _conversionService = conversionService;
            _usuarioService = usuarioService;
        }

        // Endpoint para realizar una conversión
        [HttpPost]
        public IActionResult PerformConversion([FromBody] ConversionDto request)
        {
            // Validar los datos de la solicitud
            if (request == null || request.Amount <= 0 ||
                string.IsNullOrEmpty(request.FromCurrency) || string.IsNullOrEmpty(request.ToCurrency))
            {
                return BadRequest("Datos de solicitud inválidos.");
            }

            try
            {
                // Obtener el UsuarioId desde el servicio
                var userId = _usuarioService.GetUserIdFromContext(User);

                // Ejecutar la conversión
                var result = _conversionService.ExecuteConversion(userId, request.FromCurrency, request.ToCurrency, request.Amount);

                // Obtener el historial de conversiones
                var conversions = _conversionService.GetUserConversions(userId);

                // Devolver el resultado con el historial de conversiones
                return Ok(new
                {
                    Conversion = result,
                    History = conversions
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        // Endpoint para obtener el historial de conversiones de un usuario
        [HttpGet("History")]
        public IActionResult GetUserConversions()
        {
            try
            {
                // Obtener el UsuarioId desde el servicio
                var userId = _usuarioService.GetUserIdFromContext(User);

                var conversions = _conversionService.GetUserConversions(userId);
                return Ok(conversions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
