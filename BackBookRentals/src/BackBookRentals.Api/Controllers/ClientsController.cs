using BackBookRentals.Dto.Request;
using BackBookRentals.Dto.Response;
using BackBookRentals.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace BackBookRentals.Api.Controllers
{
    [ApiController]
    [Route("api/clients")]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsService service;

        public ClientsController(IClientsService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Obtiene una lista de clientes con paginación.
        /// </summary>
        /// <param name="search">Nombre o DNI a buscar (opcional).</param>
        /// <param name="page">Número de página (opcional).</param>
        /// <param name="recordsPerPage">Registros por página (opcional).</param>
        /// <returns>Lista de clientes.</returns>
        /// <response code="200">Clientes obtenidos correctamente.</response>
        /// <response code="500">Error en el servidor.</response>
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GenericResponse<ICollection<ClientResponseDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get(
            [FromQuery] string? search, 
            [FromQuery] int page = 1,
            [FromQuery] int recordsPerPage = 10)
        {
            var pagination = new PaginationDto
            {
                Page = page,
                RecordsPerPage = recordsPerPage
            };
            var response = await service.GetAsync(search, pagination);
            return Ok(response);
        }

        /// <summary>
        /// Obtiene un cliente.
        /// </summary>
        /// <param name="id">Id del cliente.</param>
        /// <returns>cliente.</returns>
        /// <response code="200">Cliente obtenido correctamente.</response>
        /// <response code="404">Cliente no encontrado.</response>
        /// <response code="500">Error en el servidor.</response>   
        [HttpGet("{id:guid}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GenericResponse<ClientResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var response = await service.GetAsync(id);
            return Ok(response);
        }

        /// <summary>
        /// Crea un cliente.
        /// </summary>
        /// <param name="request">Datos del cliente a crear</param>
        /// <returns>Id del cliente Creado.</returns>
        /// <response code="200">Cliente creado correctamente.</response>
        /// <response code="400">Error al crear cliente.</response>
        /// <response code="500">Error en el servidor.</response>   
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Crea un cliente",
            Description = "Crea un nuevo cliente en el sistema",
            OperationId = "CreateClient",
            Tags = new[] { "Clients" }
        )]
        [SwaggerResponse(200, "Cliente creado correctamente", typeof(GenericResponse<Guid>))]
        [SwaggerResponse(400, "Error al crear cliente", typeof(BaseGenericResponse))]
        [SwaggerResponse(500, "Error en el servidor", typeof(BaseGenericResponse))]
        public async Task<IActionResult> Create([FromBody] ClientRequestDto request)
        {
            var response = await service.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Data }, response);
        }

        /// <summary>
        /// Actualizar parcialmente un cliente.
        /// </summary>
        /// <param name="id">Id del cliente.</param>
        /// <param name="request">Datos del cliente a actualizar</param>
        /// <returns>Cliente actualizado correctamente.</returns>
        /// <response code="200">Cliente actualizado correctamente.</response>
        /// <response code="400">Error al actualizar cliente.</response> 
        /// <response code="404">Cliente no encontrado.</response>
        /// <response code="500">Error en el servidor.</response> 
        [HttpPatch("{id:guid}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Actualiza un cliente",
            Description = "Actualiza parcialmente un cliente existente",
            OperationId = "UpdateClient",
            Tags = new[] { "Clients" }
        )]
        [SwaggerResponse(200, "Cliente actualizado correctamente", typeof(BaseGenericResponse))]
        [SwaggerResponse(400, "Error al actualizar cliente", typeof(BaseGenericResponse))]
        [SwaggerResponse(404, "Cliente no encontrado", typeof(BaseGenericResponse))]
        [SwaggerResponse(500, "Error en el servidor", typeof(BaseGenericResponse))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ClientUpdateRequestDto request)
        {
            var response = await service.UpdateAsync(id, request);
            return Ok(response);
        }

        /// <summary>
        /// Elimina un cliente.
        /// </summary>
        /// <param name="id">Id del cliente.</param>
        /// <returns>Cliente eliminado correctamente.</returns>
        /// <response code="200">Cliente eliminado correctamente.</response>
        /// <response code="404">Cliente no encontrado.</response>
        /// <response code="500">Error en el servidor.</response>   
        [HttpDelete("{id:guid}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var response = await service.DeleteAsync(id);
            return Ok(response);
        }
    }
}