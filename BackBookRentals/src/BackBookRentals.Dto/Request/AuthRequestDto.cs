using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace BackBookRentals.Dto.Request;

[SwaggerSchema(Title = "AuthRequestDto", Description = "Datos de autenticación del usuario")]
public class AuthRequestDto
{
    [Required(ErrorMessage = "El campo 'username' es obligatorio.")]
    [MaxLength(50, ErrorMessage = "El campo 'username' no puede exceder los 50 caracteres.")]
    [SwaggerSchema(Description = "Nombre del usuario.", Required = new[] { "UserName" })]
    public string UserName { get; set; } = default!;

    [Required(ErrorMessage = "El campo 'password' es obligatorio.")]
    [MaxLength(60, ErrorMessage = "El campo 'password' no puede exceder los 60 caracteres.")]
    [SwaggerSchema(Description = "Contraseña del usuario.", Required = new[] { "Password" })]
    public string Password { get; set; } = default!;
}