using BackBookRentals.Dto.Request;
using BackBookRentals.Repositories.Abstractions;
using Microsoft.AspNetCore.Http;
using BackBookRentals.Entities;
using BackBookRentals.Persistence;
using BackBookRentals.Repositories.Utils;
using Microsoft.EntityFrameworkCore;

namespace BackBookRentals.Repositories.Implementations;
public class ClientRepository : RepositoryBase<Client>, IClientRepository
{
    private readonly IHttpContextAccessor httpContext;

    public ClientRepository(ApplicationDbContext context, IHttpContextAccessor httpContext) : base(context)
    {
        this.httpContext = httpContext;
    }

    public async Task<ICollection<Client>> GetAsync(string? search, PaginationDto pagination)
    {
        var queryable = context.Set<Client>()
            .Where(c => string.IsNullOrEmpty(search) || c.Names.Contains(search) || c.Dni.Contains(search))
            .IgnoreQueryFilters()
            .AsNoTracking();

        await httpContext.HttpContext.InsertarPaginacionHeader(queryable);
        var clients = await queryable
            .OrderBy(c => c.Names)
            .Skip((pagination.Page - 1) * pagination.RecordsPerPage)
            .Take(pagination.RecordsPerPage)
            .ToListAsync();
        return clients;
    }
}