using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace BackBookRentals.Dto.Request;

public class BookUpdateRequestDto
{
    [MaxLength(100, ErrorMessage = "El campo 'name' no puede exceder los 100 caracteres.")]
    [SwaggerSchema(Description = "Nombre del libro.")]
    public string? Name { get; set; } = default!;

    [MaxLength(100, ErrorMessage = "El campo 'author' no puede exceder los 100 caracteres.")]
    [SwaggerSchema(Description = "Autor del libro.")]
    public string? Author { get; set; } = default!;

    [StringLength(13, MinimumLength = 10, ErrorMessage = "El ISBN debe tener entre 10 y 13 caracteres.")]
    [RegularExpression(@"^\d{10}(\d{3})?$", ErrorMessage = "El ISBN debe contener solo números y tener 10 o 13 dígitos.")]
    [SwaggerSchema(Description = "ISBN del libro (10 o 13 dígitos numéricos).")]
    public string? Isbn { get; set; } = default!;

    [SwaggerSchema(Description = "Estado del libro (true = activo, false = inactivo).")]
    public bool? Status { get; set; }
}