using BackBookRentals.Dto.Request;
using BackBookRentals.Entities;
using System.Threading.Tasks;

namespace BackBookRentals.Repositories.Abstractions;

public interface IOrderRepository : IRepositoryBase<Order>
{
    Task<ICollection<Book>> GetBooksByClientDniAsync(string dni);
    Task<ICollection<Order>> GetOrdersByClientIdAsync(Guid clientId);
    Task<ICollection<Order>> GetOrdersByBookIdAsync(Guid bookId);
    Task<ICollection<Order>> GetAsync(PaginationDto pagination);
}