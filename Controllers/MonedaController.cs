using Conversor_Monedas_Api.DTOs;
using Conversor_Monedas_Api.Interfaces.services;
using Microsoft.AspNetCore.Mvc;

namespace Conversor_Monedas_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonedaController : ControllerBase
    {
        private readonly IMonedaService _monedaService;

        public MonedaController(IMonedaService monedaService)
        {
            _monedaService = monedaService;
        }

        [HttpGet("All")]
        public IActionResult GetAllCurrencies()
        {
            List<MonedaDto> currencies = _monedaService.GetAllCurrencies();
            return Ok(currencies);
        }

        [HttpGet("{id}")]
        public IActionResult GetCurrencyById(int id)
        {
            var currency = _monedaService.GetCurrencyById(id);
            if (currency == null)
            {
                return NotFound();
            }
            return Ok(currency);
        }

        [HttpPost]
        public IActionResult AddCurrency([FromBody] MonedaDto monedaDto)
        {
            if (monedaDto == null || string.IsNullOrEmpty(monedaDto.Code))
            {
                return BadRequest();
            }

            int currencyId = _monedaService.AddCurrency(monedaDto);
            return CreatedAtAction(nameof(GetCurrencyById), new { id = currencyId });//201
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCurrency(int id, [FromBody] MonedaDto currencyDto)
        {
            try
            {
                if (currencyDto == null || id <= 0)
                {
                    return BadRequest(new { Message = "Datos inválidos para la actualización." });
                }

                var isUpdated = _monedaService.UpdateCurrency(id, currencyDto);

                if (!isUpdated)
                {
                    return NotFound(new { Message = $"Moneda con ID {id} no encontrada." });
                }

                return Ok(new { Message = "Moneda actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCurrency(int id)
        {
            bool success = _monedaService.DeleteCurrency(id);
            if (!success)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
