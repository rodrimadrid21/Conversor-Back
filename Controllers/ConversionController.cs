using Conversor_Monedas_Api.DTOs;
using Conversor_Monedas_Api.Interfaces.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Conversor_Monedas_Api.Controllers
{
    [Authorize]
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

        [HttpPost]
        public IActionResult Convert([FromBody] ConversionRequestDto request)
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
                var userId = _usuarioService.GetUserIdFromContext(User);

                // 2) Ejecutar conversión
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
                // 401
                return Unauthorized(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // 403- limite de conversiones
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "Ocurrió un error al realizar la conversión.",
                    Detail = ex.Message
                });
            }
        }

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
