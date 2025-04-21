using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace BackBookRentals.Dto.Request;

public class OrderRequestDto
{
    [Required]
    [SwaggerSchema(Description = "Id del cliente.")]
    public Guid ClientId { get; set; }
    [Required]
    [SwaggerSchema(Description = "Ids de los libros.")]
    public List<Guid> BookIds { get; set; } = new();
}