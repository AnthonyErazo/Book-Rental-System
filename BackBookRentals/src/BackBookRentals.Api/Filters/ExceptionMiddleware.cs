using BackBookRentals.Dto.Exception;
using BackBookRentals.Dto.Response;
using System.Net;
using System.Text.Json;

namespace BackBookRentals.Api.Filters;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ResponseException ex)
        {
            _logger.LogWarning($"Error controlado: {ex.Message}");
            await HandleExceptionAsync(context, ex.StatusCode, ex.Message);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Error inesperado.");
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "Error interno del servidor.");
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        var response = new BaseGenericResponse
        {
            Success = false,
            Message = message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}