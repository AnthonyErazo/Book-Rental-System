using BackBookRentals.Entities;
using BackBookRentals.Persistence;
using BackBookRentals.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BackBookRentals.Repositories.Implementations;

public class UserTokenRepository: RepositoryBase<UserToken>, IUserTokenRepository
{
    public UserTokenRepository(ApplicationDbContext context) : base(context) { }

    public async Task<UserToken?> GetValidTokenAsync(Guid userId, string tokenId)
    {
        return await context.Set<UserToken>()
            .FirstOrDefaultAsync(t => t.UserId == userId && t.TokenId == tokenId);
    }

    public async Task InvalidateTokenAsync(string tokenId)
    {
        var token = await context.Set<UserToken>()
            .FirstOrDefaultAsync(t => t.TokenId == tokenId);

        context.Set<UserToken>().Remove(token);
        await context.SaveChangesAsync();
    }


    public async Task InvalidateAllTokensAsync(Guid userId)
    {
        var tokens = await context.Set<UserToken>()
            .Where(t => t.UserId == userId)
            .ToListAsync();

        context.Set<UserToken>().RemoveRange(tokens);
        await context.SaveChangesAsync();
    }

}