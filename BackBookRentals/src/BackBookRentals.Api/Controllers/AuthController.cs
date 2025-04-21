using BackBookRentals.Dto.Request;
using BackBookRentals.Dto.Response;
using BackBookRentals.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace BackBookRentals.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IUserService service;

        public AuthController(IUserService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Logeo de un usuario.
        /// </summary>
        /// <param name="request">Credenciales del usuario</param>
        /// <returns>Token de sesion.</returns>
        /// <response code="200">Token obtenido correctamente.</response>
        /// <response code="400">Credenciales inválidas o usuario no encontrado.</response>
        /// <response code="500">Error en el servidor.</response>
        [HttpPost]
        [Route("login")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Logeo de un usuario",
            Description = "Autentica un usuario y retorna un token de sesión",
            OperationId = "Login",
            Tags = new[] { "Auth" }
        )]
        [SwaggerResponse(200, "Token obtenido correctamente", typeof(GenericResponse<LoginResponseDto>))]
        [SwaggerResponse(400, "Credenciales inválidas o usuario no encontrado", typeof(BaseGenericResponse))]
        [SwaggerResponse(500, "Error en el servidor", typeof(BaseGenericResponse))]
        public async Task<IActionResult> Login([FromBody][Required] AuthRequestDto request)
        {
            var response = await service.LoginAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Registro de un usuario.
        /// </summary>
        /// <param name="request">Datos del nuevo usuario</param>
        /// <returns>Registro exitoso.</returns>
        /// <response code="200">Registro exitoso.</response>
        /// <response code="400">Error al registrarse (usuario ya existe o datos inválidos).</response>
        /// <response code="500">Error en el servidor.</response>
        [HttpPost]
        [Route("register")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Registro de un usuario",
            Description = "Registra un nuevo usuario en el sistema",
            OperationId = "Register",
            Tags = new[] { "Auth" }
        )]
        [SwaggerResponse(200, "Registro exitoso", typeof(BaseGenericResponse))]
        [SwaggerResponse(400, "Error al registrarse (usuario ya existe o datos inválidos)", typeof(BaseGenericResponse))]
        [SwaggerResponse(500, "Error en el servidor", typeof(BaseGenericResponse))]
        public async Task<IActionResult> Register([FromBody][Required] AuthRequestDto request)
        {
            var response = await service.RegisterAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Cierre de sesion.
        /// </summary>
        /// <returns>Sesion cerrada.</returns>
        /// <response code="200">Sesion cerrada.</response>
        /// <response code="400">No se encontro sesion activa.</response>
        /// <response code="500">Error en el servidor.</response>
        [HttpPost]
        [Authorize]
        [Route("logout")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Logout()
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);
            var jti = User.FindFirst("tokenId")!.Value;

            var response = await service.LogoutAsync(userId, jti);
            return Ok(response);
        }

        /// <summary>
        /// Cierre de todas las sesiones.
        /// </summary>
        /// <returns>Sesiones cerradas.</returns>
        /// <response code="200">Sesiones cerradas.</response>
        /// <response code="400">No se encontraron sesiones activas.</response>
        /// <response code="500">Error en el servidor.</response>
        [HttpPost]
        [Authorize]
        [Route("logoutAll")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> LogoutAll()
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);
            var response = await service.LogoutAllAsync(userId);
            return Ok(response);
        }
    }
}