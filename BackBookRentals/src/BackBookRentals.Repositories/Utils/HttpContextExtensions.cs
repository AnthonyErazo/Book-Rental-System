using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BackBookRentals.Repositories.Utils;

public static class HttpContextExtensions
{
    public static async Task InsertarPaginacionHeader<T>(this HttpContext httpContext, IQueryable<T> queryable)
    {
        if (httpContext == null)
            throw new ArgumentNullException(nameof(httpContext));

        var totalRecords = await queryable.CountAsync();
        httpContext.Response.Headers.Add("TotalRecordsQuantity", totalRecords.ToString());
    }
}