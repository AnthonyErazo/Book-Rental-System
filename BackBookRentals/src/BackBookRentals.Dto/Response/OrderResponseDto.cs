using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace BackBookRentals.Dto.Response;

public class OrderResponseDto
{
    [Required]
    [SwaggerSchema(Description = "Id de la orden.")]
    public Guid Id { get; set; }
    [Required]
    [SwaggerSchema(Description = "Fecha de registro de la orden.")]
    public DateTime RegisterTime { get; set; }
    [Required]
    [SwaggerSchema(Description = "Estado de la orden.")]
    public bool Status { get; set; }
    [Required]
    [SwaggerSchema(Description = "Cliente de la orden.")]
    public ClientResponseDto Client { get; set; }
    [Required]
    [SwaggerSchema(Description = "Libros de la orden.")]
    public ICollection<BookResponseDto> Books { get; set; }
}
