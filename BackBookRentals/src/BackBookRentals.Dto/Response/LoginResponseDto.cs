using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace BackBookRentals.Dto.Response;

public class LoginResponseDto
{
    [SwaggerSchema(Description = "Token de sesion del usuario.")]
    [Required]
    public string Token { get; set; } = default!;

    [SwaggerSchema(Description = "Fecha de expiracion del token.")]
    [Required]
    public DateTime ExpirationDate { get; set; }
}