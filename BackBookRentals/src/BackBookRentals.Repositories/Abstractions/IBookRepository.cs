using BackBookRentals.Dto.Request;
using BackBookRentals.Entities;

namespace BackBookRentals.Repositories.Abstractions;

public interface IBookRepository : IRepositoryBase<Book>
{
    Task<ICollection<Book>> GetAsync(string? search, PaginationDto pagination);
}