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
    [Route("api/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService service;

        public OrdersController(IOrderService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Obtiene una lista de órdenes con paginación.
        /// </summary>
        /// <param name="page">Número de página (opcional).</param>
        /// <param name="recordsPerPage">Registros por página (opcional).</param>
        /// <returns>Lista de órdenes.</returns>
        /// <response code="200">Órdenes obtenidas correctamente.</response>
        /// <response code="500">Error en el servidor.</response>
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GenericResponse<ICollection<OrderResponseDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get(
            [FromQuery] int page = 1,
            [FromQuery] int recordsPerPage = 10)
        {
            var pagination = new PaginationDto
            {
                Page = page,
                RecordsPerPage = recordsPerPage
            };
            var response = await service.GetAsync(pagination);
            return Ok(response);
        }

        /// <summary>
        /// Crea una nueva orden.
        /// </summary>
        /// <param name="request">Datos de la orden a crear</param>
        /// <returns>Id de la orden creada.</returns>
        /// <response code="201">Orden creada correctamente.</response>
        /// <response code="400">Error al crear la orden.</response>
        /// <response code="500">Error en el servidor.</response>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Crea una orden",
            Description = "Crea una nueva orden en el sistema",
            OperationId = "CreateOrder",
            Tags = new[] { "Orders" }
        )]
        [SwaggerResponse(201, "Orden creada correctamente", typeof(GenericResponse<Guid>))]
        [SwaggerResponse(400, "Error al crear la orden", typeof(BaseGenericResponse))]
        [SwaggerResponse(500, "Error en el servidor", typeof(BaseGenericResponse))]
        public async Task<IActionResult> Create([FromBody] OrderRequestDto request)
        {
            var result = await service.CreateAsync(request);
            return CreatedAtAction(nameof(Create), new { id = result.Data }, result);
        }

        /// <summary>
        /// Obtiene los libros prestados por un cliente usando su DNI.
        /// </summary>
        /// <param name="dni">DNI del cliente.</param>
        /// <returns>Lista de libros prestados.</returns>
        /// <response code="200">Libros obtenidos correctamente.</response>
        /// <response code="404">Cliente no encontrado.</response>
        /// <response code="500">Error en el servidor.</response>
        [HttpGet("client/{dni}/books")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GenericResponse<ICollection<BookResponseDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetBooksByClientDni(string dni)
        {
            var result = await service.GetBooksByDniAsync(dni);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene las órdenes de un cliente específico.
        /// </summary>
        /// <param name="clientId">Id del cliente.</param>
        /// <returns>Lista de órdenes del cliente.</returns>
        /// <response code="200">Órdenes obtenidas correctamente.</response>
        /// <response code="404">Cliente no encontrado.</response>
        /// <response code="500">Error en el servidor.</response>
        [HttpGet("client/{clientId}/orders")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GenericResponse<ICollection<OrderByClientResponseDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetOrdersByClientId(Guid clientId)
        {
            var result = await service.GetOrdersByClientIdAsync(clientId);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene las órdenes de un libro específico.
        /// </summary>
        /// <param name="bookId">Id del libro.</param>
        /// <returns>Lista de órdenes del libro.</returns>
        /// <response code="200">Órdenes obtenidas correctamente.</response>
        /// <response code="404">Libro no encontrado.</response>
        /// <response code="500">Error en el servidor.</response>
        [HttpGet("book/{bookId}/orders")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GenericResponse<ICollection<OrderByBookResponseDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetOrdersByBookId(Guid bookId)
        {
            var result = await service.GetOrdersByBookIdAsync(bookId);
            return Ok(result);
        }

        /// <summary>
        /// Actualiza el estado de una orden.
        /// </summary>
        /// <param name="orderId">Id de la orden.</param>
        /// <returns>Orden actualizada correctamente.</returns>
        /// <response code="200">Orden actualizada correctamente.</response>
        /// <response code="404">Orden no encontrada.</response>
        /// <response code="500">Error en el servidor.</response>
        [HttpPatch("{orderId}/status")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateStatus(Guid orderId)
        {
            var result = await service.UpdateStatusAsync(orderId);
            return Ok(result);
        }

        /// <summary>
        /// Elimina una orden.
        /// </summary>
        /// <param name="orderId">Id de la orden.</param>
        /// <returns>Orden eliminada correctamente.</returns>
        /// <response code="200">Orden eliminada correctamente.</response>
        /// <response code="404">Orden no encontrada.</response>
        /// <response code="500">Error en el servidor.</response>
        [HttpDelete("{orderId}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete(Guid orderId)
        {
            var result = await service.DeleteAsync(orderId);
            return Ok(result);
        }
    }
}
