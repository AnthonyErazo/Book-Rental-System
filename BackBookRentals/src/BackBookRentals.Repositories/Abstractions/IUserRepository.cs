using BackBookRentals.Entities;

namespace BackBookRentals.Repositories.Abstractions;

public interface IUserRepository: IRepositoryBase<User>
{
    Task<User?> GetByUserNameAsync(string username);
}