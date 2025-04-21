using BackBookRentals.Dto.Request;
using BackBookRentals.Dto.Response;

namespace BackBookRentals.Services.Abstractions;

public interface IOrderService
{
    Task<GenericResponse<ICollection<OrderResponseDto>>> GetAsync(PaginationDto pagination);
    Task<GenericResponse<Guid>> CreateAsync(OrderRequestDto request);
    Task<GenericResponse<ICollection<BookResponseDto>>> GetBooksByDniAsync(string dni);
    Task<GenericResponse<ICollection<OrderByClientResponseDto>>> GetOrdersByClientIdAsync(Guid clientId);
    Task<GenericResponse<ICollection<OrderByBookResponseDto>>> GetOrdersByBookIdAsync(Guid bookId);
    Task<BaseGenericResponse> UpdateStatusAsync(Guid orderId);
    Task<BaseGenericResponse> DeleteAsync(Guid orderId);
}