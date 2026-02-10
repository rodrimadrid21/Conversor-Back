using Conversor_Monedas_Api.DTOs;
using Conversor_Monedas_Api.Interfaces.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Conversor_Monedas_Api.Controllers
{
    [Authorize] // 👈 ahora sí: todos los endpoints requieren JWT válido
    [Route("api/[controller]")]
    [ApiController]
    public class ConversionController : ControllerBase
    {
        private readonly IConversionService _conversionService;
        private readonly IUsuarioService _usuarioService;

        public ConversionController(IConversionService conversionService, IUsuarioService usuarioService)
        {
            _conversionService = conversionService;
            _usuarioService = usuarioService;
        }

        // POST api/Conversion
        [HttpPost]
        public IActionResult PerformConversion([FromBody] ConversionRequestDto request)//para ver la request deserializada (JSON) "Console.WriteLine(JsonSerializer.Serialize(request));"
        {
            if (request == null ||
                request.Amount <= 0 ||
                string.IsNullOrWhiteSpace(request.FromCurrency) ||
                string.IsNullOrWhiteSpace(request.ToCurrency))
            {
                return BadRequest(new { Message = "Datos de solicitud inválidos." });
            }

            try
            {
                // 1) UserId desde el token
                var userId = _usuarioService.GetUserIdFromContext(User); //“Aunque el front mande usuarioId, el backend no confía en eso. Obtiene el userId desde el JWT (claims).”

                // 2) Ejecutar conversión (valida límite internamente)
                var result = _conversionService.ExecuteConversion(
                    userId,
                    request.FromCurrency,
                    request.ToCurrency,
                    request.Amount
                );

                // 3) Traer historial actualizado
                var conversions = _conversionService.GetUserConversions(userId);

                return Ok(new
                {
                    Conversion = result,
                    History = conversions
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                // Cuando ValidarLimiteSuscripcion lanza “Usuario no válido o inactivo”
                return Unauthorized(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // 👉 Límite de suscripción alcanzado
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Error inesperado
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "Ocurrió un error al realizar la conversión.",
                    Detail = ex.Message
                });
            }
        }

        // GET api/Conversion/History
        [HttpGet("History")]
        public IActionResult GetUserConversions()
        {
            try
            {
                var userId = _usuarioService.GetUserIdFromContext(User);

                var conversions = _conversionService.GetUserConversions(userId);
                return Ok(conversions);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "Ocurrió un error al obtener el historial de conversiones.",
                    Detail = ex.Message
                });
            }
        }
    }
}
