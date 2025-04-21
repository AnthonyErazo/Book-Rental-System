using System.ComponentModel.DataAnnotations;

namespace BackBookRentals.Dto.Response;

public class GenericResponse<T> : BaseGenericResponse
{
    public T? Data { get; set; }
}