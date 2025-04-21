using BackBookRentals.Entities;
using BackBookRentals.Dto.Request;

namespace BackBookRentals.Repositories.Abstractions;

public interface IClientRepository : IRepositoryBase<Client>
{
    Task<ICollection<Client>> GetAsync(string? search, PaginationDto pagination);
}