using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace BackBookRentals.Dto.Response;

public class OrderByBookResponseDto
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
}