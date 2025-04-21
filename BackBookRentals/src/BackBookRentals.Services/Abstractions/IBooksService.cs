using BackBookRentals.Dto.Request;
using BackBookRentals.Dto.Response;

namespace BackBookRentals.Services.Abstractions;

public interface IBooksService
{
    Task<GenericResponse<ICollection<BookResponseDto>>> GetAsync(string? search, PaginationDto pagination);
    Task<GenericResponse<BookResponseDto>> GetAsync(Guid id);
    Task<GenericResponse<Guid>> AddAsync(BookRequestDto request);
    Task<BaseGenericResponse> UpdateAsync(Guid id, BookUpdateRequestDto request);
    Task<BaseGenericResponse> DeleteAsync(Guid id);
}