using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace BackBookRentals.Dto.Response;

public class BookResponseDto
{
    [Required]
    [SwaggerSchema(Description = "Identificador único del libro.", ReadOnly = true)]
    public Guid BookId { get; set; }

    [Required(ErrorMessage = "El campo 'name' es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El campo 'name' no puede exceder los 100 caracteres.")]
    [SwaggerSchema(Description = "Nombre del libro.")]
    public string Name { get; set; } = default!;

    [Required(ErrorMessage = "El campo 'author' es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El campo 'author' no puede exceder los 100 caracteres.")]
    [SwaggerSchema(Description = "Autor del libro.")]
    public string Author { get; set; } = default!;

    [Required(ErrorMessage = "El campo 'isbn' es obligatorio.")]
    [StringLength(13, MinimumLength = 10, ErrorMessage = "El ISBN debe tener entre 10 y 13 caracteres.")]
    [RegularExpression(@"^\d{10}(\d{3})?$", ErrorMessage = "El ISBN debe contener solo números y tener 10 o 13 dígitos.")]
    [SwaggerSchema(Description = "ISBN del libro (10 o 13 dígitos numéricos).")]
    public string Isbn { get; set; } = default!;

    [SwaggerSchema(Description = "Estado del libro (true = activo, false = inactivo).")]
    [Required]
    public bool Status { get; set; }
}