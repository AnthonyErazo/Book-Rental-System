using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace BackBookRentals.Dto.Request;

public class ClientUpdateRequestDto
{
    [MaxLength(100, ErrorMessage = "El campo 'Names' no puede exceder los 100 caracteres.")]
    [SwaggerSchema(Description = "Nombres del cliente.", Nullable = true)]
    public string? Names { get; set; }

    [MaxLength(100, ErrorMessage = "El campo 'LastNames' no puede exceder los 100 caracteres.")]
    [SwaggerSchema(Description = "Apellidos del cliente.", Nullable = true)]
    public string? LastNames { get; set; }

    [StringLength(8, MinimumLength = 8, ErrorMessage = "El 'Dni' debe tener 8 caracteres.")]
    [SwaggerSchema(Description = "Número de DNI del cliente (8 caracteres).", Nullable = true)]
    public string? Dni { get; set; }

    [Range(18, 120, ErrorMessage = "La edad debe estar entre 18 y 120 años.")]
    [SwaggerSchema(Description = "Edad del cliente en años.", Nullable = true)]
    public int? Age { get; set; }
}