using BackBookRentals.Dto.Request;
using BackBookRentals.Dto.Response;

namespace BackBookRentals.Services.Abstractions;

public interface IClientsService
{
    Task<GenericResponse<ICollection<ClientResponseDto>>> GetAsync(string? search, PaginationDto pagination);
    Task<GenericResponse<ClientResponseDto>> GetAsync(Guid id);
    Task<GenericResponse<Guid>> AddAsync(ClientRequestDto request);
    Task<BaseGenericResponse> UpdateAsync(Guid id, ClientUpdateRequestDto request);
    Task<BaseGenericResponse> DeleteAsync(Guid id);
}