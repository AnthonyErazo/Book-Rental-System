using System.ComponentModel.DataAnnotations;

namespace BackBookRentals.Dto.Response;

public class BaseGenericResponse
{
    [Required]
    public bool Success { get; set; } = false;
    
    [Required]
    public string Message { get; set; } = default!;
}