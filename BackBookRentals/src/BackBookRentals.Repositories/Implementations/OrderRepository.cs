using BackBookRentals.Dto.Request;
using BackBookRentals.Entities;
using BackBookRentals.Persistence;
using BackBookRentals.Repositories.Abstractions;
using BackBookRentals.Repositories.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BackBookRentals.Repositories.Implementations;

public class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    private readonly IHttpContextAccessor httpContext;

    public OrderRepository(ApplicationDbContext context, IHttpContextAccessor httpContext) : base(context)
    {
        this.httpContext = httpContext;
    }

    public async Task<ICollection<Book>> GetBooksByClientDniAsync(string dni)
    {
        return await context.Set<Order>()
            .Where(o => o.Client.Dni == dni)
            .SelectMany(o => o.OrderBooks.Select(ob => ob.Book))
            .Distinct()
            .ToListAsync();
    }
    public async Task<ICollection<Order>> GetOrdersByClientIdAsync(Guid clientId)
    {
        return await context.Set<Order>()
            .Where(o => o.ClientId == clientId)
            .ToListAsync();
    }

    public async Task<ICollection<Order>> GetOrdersByBookIdAsync(Guid bookId)
    {
        return await context.Set<OrderBook>()
            .Where(ob => ob.BookId == bookId)
            .Select(ob => ob.Order)
            .Distinct()
            .ToListAsync();
    }
    public async Task<ICollection<Order>> GetAsync(PaginationDto pagination)
    {
        var queryable = context.Set<Order>()
        .IgnoreQueryFilters()
        .AsNoTracking();

        await httpContext.HttpContext.InsertarPaginacionHeader(queryable);
        var clients = await queryable
            .OrderBy(c => c.RegisterTime)
            .Skip((pagination.Page - 1) * pagination.RecordsPerPage)
            .Take(pagination.RecordsPerPage)
            .ToListAsync();
        return clients;
    }
}