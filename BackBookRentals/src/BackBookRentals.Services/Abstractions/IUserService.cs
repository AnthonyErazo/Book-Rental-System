using BackBookRentals.Dto.Request;
using BackBookRentals.Dto.Response;

namespace BackBookRentals.Services.Abstractions;

public interface IUserService
{
    Task<BaseGenericResponse> RegisterAsync(AuthRequestDto request);
    Task<GenericResponse<LoginResponseDto>> LoginAsync(AuthRequestDto request);
    Task<BaseGenericResponse> LogoutAsync(Guid userId, string jti);
    Task<BaseGenericResponse> LogoutAllAsync(Guid userId);
}