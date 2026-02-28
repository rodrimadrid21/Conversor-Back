using Conversor_Monedas_Api.DTOs;
using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Interfaces.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Conversor_Monedas_Api.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUsuarioService _userService;

        public AuthenticationController(IUsuarioService userService)
        {
            _userService = userService;
        }
        
        [HttpPost("login")]
        public IActionResult Authenticate([FromBody] AuthenticationDto credentials)
        {
            var token = _userService.Authenticate(credentials);

            if (token == null)
            {
                return Unauthorized(new LoginResponseDto
                {
                    Status = "error",
                    Mensaje = "Credenciales incorrectas"
                });
            }

            Console.WriteLine($"Token recibido: {token}");
            // Retornar el token
            return Ok(new LoginResponseDto
            {
                Status = "success",
                Mensaje = "Inicio de sesión exitoso",
                Token = token,
            });
        }
    }
}
