using Conversor_Monedas_Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Conversor_Monedas_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SomeController : ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;

        public SomeController(IConfiguration configuration)
        {
            _jwtTokenService = new JwtTokenService(configuration);
        }

        [HttpPost("verify-token")]
        public IActionResult VerifyToken([FromBody] dynamic body)
        {
            string token = body.token;
            bool isValid = _jwtTokenService.ValidateJwtToken(token);

            if (isValid)
            {
                return Ok("Token válido");
            }
            else
            {
                return Unauthorized("Token inválido");
            }
        }
    }
}
