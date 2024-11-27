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
            // Paso 1: Validar las credenciales y generar el token
            var token = _userService.Authenticate(credentials);

            if (token == null)
            {
                // Devuelve un error con el estado y mensaje según tu interfaz `ResLogin`
                return Unauthorized(new ResLogin
                {
                    Status = "error",
                    Mensaje = "Credenciales incorrectas"
                });
            }
            Console.WriteLine($"Token recibido: {token}");


            // Paso 2: Retornar el token
            return Ok(new ResLogin
            {
                Status = "success",
                Mensaje = "Inicio de sesión exitoso",
                Token = token,
            });
        }
    }
}
