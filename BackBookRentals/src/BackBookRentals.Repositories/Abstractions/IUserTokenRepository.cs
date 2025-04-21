using BackBookRentals.Entities;

namespace BackBookRentals.Repositories.Abstractions;

public interface IUserTokenRepository: IRepositoryBase<UserToken>
{
    Task<UserToken?> GetValidTokenAsync(Guid userId, string tokenId);
    Task InvalidateTokenAsync(string tokenId);
    Task InvalidateAllTokensAsync(Guid userId);
}