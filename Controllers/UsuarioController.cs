using Conversor_Monedas_Api.DTOs;
using Conversor_Monedas_Api.Entities;
using Conversor_Monedas_Api.Enum;
using Conversor_Monedas_Api.Interfaces.services;
using Conversor_Monedas_Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Conversor_Monedas_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _userService;
        public UsuarioController(IUsuarioService userService)
        {
            _userService = userService;
        }

        [HttpGet("All")]
        public IActionResult GetAllActiveUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] UsuarioDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.UserName) || string.IsNullOrEmpty(userDto.Password))
            {
                return BadRequest(new RegisterResponseDto { Mensaje = "El nombre de usuario y la contraseña son obligatorios." });
            }

            // Llama al servicio para registrar al usuario
            int userId = _userService.RegisterUser(userDto);

            if (userId <= 0)
            {
                return BadRequest(new RegisterResponseDto { Mensaje = "Error al registrar el usuario." });
            }

            // Retorna mensaje de éxito
            return Ok(new RegisterResponseDto { Mensaje = "Registro exitoso." });
        }

        // Obtener usuario por ID
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { Message = "Usuario no encontrado." });
            }
            return Ok(user);
        }

        // Obtener usuario por nombre de usuario
        // 🔥 FIX aplicado acá
        [HttpGet("ByUsername/{username}")]
        public IActionResult GetUserByUsername(string username)
        {
            var user = _userService.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound(new { Message = "Usuario no encontrado." });
            }
            return Ok(user);
        }

        // Actualizar usuario
        [HttpPut]
        public IActionResult UpdateUser([FromBody] UsuarioDto userDto)
        {
            try
            {
                bool updated = _userService.UpdateUser(userDto);
                if (!updated)
                {
                    return NotFound(new { Message = "Usuario no encontrado para actualizar." });
                }
                return Ok(new { Message = "Usuario actualizado exitosamente." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // delete del usuario
        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            bool deleted = _userService.DeleteUser(userId);
            if (!deleted)
            {
                return NotFound(new { Message = "Usuario no encontrado para eliminación." });
            }
            return Ok(new { Message = "Usuario eliminado lógicamente." });
        }

        // Actualizar la suscripción del usuario
        [HttpPost("activar-plan")]
        public IActionResult ActivarPlan([FromBody] SuscripcionDto dto)
        {
            try
            {
                // Sacamos el userId del token
                int userId = _userService.GetUserIdFromContext(HttpContext.User);

                bool ok = _userService.UpdateUserSubscription(userId, dto.Type);

                if (!ok)
                    return NotFound(new { message = "Usuario no encontrado" });

                return Ok(new { message = $"Suscripción actualizada a {dto.Type}" });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Usuario no autenticado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
