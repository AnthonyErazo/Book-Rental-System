using BackBookRentals.Dto.Request;
using BackBookRentals.Repositories.Abstractions;
using Microsoft.AspNetCore.Http;
using BackBookRentals.Entities;
using BackBookRentals.Persistence;
using BackBookRentals.Repositories.Utils;
using Microsoft.EntityFrameworkCore;

namespace BackBookRentals.Repositories.Implementations;
public class BookRepository : RepositoryBase<Book>, IBookRepository
{
    private readonly IHttpContextAccessor httpContext;

    public BookRepository(ApplicationDbContext context, IHttpContextAccessor httpContext) : base(context)
    {
        this.httpContext = httpContext;
    }

    public async Task<ICollection<Book>> GetAsync(string? search, PaginationDto pagination)
    {
        var queryable = context.Set<Book>()
            .Where(c => string.IsNullOrEmpty(search) || c.Name.Contains(search) || c.Isbn.Contains(search))
            .IgnoreQueryFilters()
            .AsNoTracking();

        await httpContext.HttpContext.InsertarPaginacionHeader(queryable);
        var clients = await queryable
            .OrderBy(c => c.Name)
            .Skip((pagination.Page - 1) * pagination.RecordsPerPage)
            .Take(pagination.RecordsPerPage)
            .ToListAsync();
        return clients;
    }
}