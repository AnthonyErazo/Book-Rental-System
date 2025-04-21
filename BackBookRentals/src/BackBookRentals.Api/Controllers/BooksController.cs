using BackBookRentals.Dto.Request;
using BackBookRentals.Dto.Response;
using BackBookRentals.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BackBookRentals.Api.Controllers
{
    [ApiController]
    [Route("api/books")]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBooksService service;

        public BooksController(IBooksService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Obtiene una lista de libros con paginación.
        /// </summary>
        /// <param name="search">Nombre o ISBN a buscar (opcional).</param>
        /// <param name="page">Número de página (opcional).</param>
        /// <param name="recordsPerPage">Registros por página (opcional).</param>
        /// <returns>Lista de libros.</returns>
        /// <response code="200">Libros obtenidos correctamente.</response>
        /// <response code="500">Error en el servidor.</response>
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GenericResponse<ICollection<BookResponseDto>>), (int)HttpStatusCode.OK)]
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
        /// Obtiene un libro.
        /// </summary>
        /// <param name="id">Id del libro.</param>
        /// <returns>libro.</returns>
        /// <response code="200">Libro obtenido correctamente.</response>
        /// <response code="404">Libro no encontrado.</response>
        /// <response code="500">Error en el servidor.</response>   
        [HttpGet("{id:guid}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GenericResponse<BookResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await service.GetAsync(id);
            return Ok(response);
        }

        /// <summary>
        /// Crea un libro.
        /// </summary>
        /// <returns>Id del libro Creado.</returns>
        /// <response code="200">Libro creado correctamente.</response>
        /// <response code="400">Error al crear libro.</response>
        /// <response code="500">Error en el servidor.</response>   
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GenericResponse<Guid>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Create([FromBody] BookRequestDto request)
        {
            var response = await service.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Data }, response);
        }

        /// <summary>
        /// Actualizar parcialmente un libro.
        /// </summary>
        /// <param name="id">Id del libro.</param>
        /// <returns>Libro actualizado correctamente.</returns>
        /// <response code="200">Libro actualizado correctamente.</response>
        /// /// <response code="400">Error al actualizar libro.</response> 
        /// <response code="404">Libro no encontrado.</response>
        /// <response code="500">Error en el servidor.</response> 
        [HttpPatch("{id:guid}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] BookUpdateRequestDto request)
        {
            var response = await service.UpdateAsync(id, request);
            return Ok(response);
        }

        /// <summary>
        /// Elimina un libro.
        /// </summary>
        /// <param name="id">Id del libro.</param>
        /// <returns>Libro eliminado correctamente.</returns>
        /// <response code="200">Libro eliminado correctamente.</response>
        /// <response code="404">Libro no encontrado.</response>
        /// <response code="500">Error en el servidor.</response>   
        [HttpDelete("{id:guid}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseGenericResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await service.DeleteAsync(id);
            return Ok(response);
        }
    }
}