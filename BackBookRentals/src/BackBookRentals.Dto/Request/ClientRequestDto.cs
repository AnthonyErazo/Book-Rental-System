using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace BackBookRentals.Dto.Request;

public class ClientRequestDto
{

    [Required(ErrorMessage = "El campo 'names' es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El campo 'Names' no puede exceder los 100 caracteres.")]
    [SwaggerSchema(Description = "Nombres del cliente.")]
    public string Names { get; set; } = default!;

    [Required(ErrorMessage = "El campo 'lastNames' es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El campo 'LastNames' no puede exceder los 100 caracteres.")]
    [SwaggerSchema(Description = "Apellidos del cliente.")]
    public string LastNames { get; set; } = default!;

    [Required(ErrorMessage = "El campo 'dni' es obligatorio.")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "El 'Dni' debe tener 8 caracteres.")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe contener solo 8 dígitos numéricos.")]
    [SwaggerSchema(Description = "Número de DNI del cliente (8 dígitos numéricos).")]
    public string Dni { get; set; } = default!;

    [Required(ErrorMessage = "El campo 'age' es obligatorio.")]
    [Range(18, 120, ErrorMessage = "La edad debe estar entre 18 y 120 años.")]
    [SwaggerSchema(Description = "Edad del cliente en años.")]
    public int Age { get; set; }
}