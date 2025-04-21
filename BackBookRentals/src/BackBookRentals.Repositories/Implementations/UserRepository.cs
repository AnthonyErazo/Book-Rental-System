using BackBookRentals.Entities;
using BackBookRentals.Persistence;
using BackBookRentals.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BackBookRentals.Repositories.Implementations;

public class UserRepository: RepositoryBase<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) { }

    public async Task<User?> GetByUserNameAsync(string username)
    {
        return await context.Set<User>().FirstOrDefaultAsync(u => u.UserName == username);
    }

}