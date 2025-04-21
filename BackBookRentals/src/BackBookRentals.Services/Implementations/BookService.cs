using AutoMapper;
using BackBookRentals.Dto.Request;
using BackBookRentals.Repositories.Abstractions;
using BackBookRentals.Services.Abstractions;
using Microsoft.Extensions.Logging;
using BackBookRentals.Dto.Exception;
using BackBookRentals.Dto.Response;
using BackBookRentals.Entities;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace BackBookRentals.Services.Implementations;

public class BookService : IBooksService
{
    private readonly IBookRepository repository;
    private readonly ILogger<BookService> logger;
    private readonly IMapper mapper;

    public BookService(
        IBookRepository repository,
        ILogger<BookService> logger,
        IMapper mapper
        )
    {
        this.repository = repository;
        this.logger = logger;
        this.mapper = mapper;
    }
    public async Task<GenericResponse<ICollection<BookResponseDto>>> GetAsync(string? search, PaginationDto pagination)
    {
        var data = await repository.GetAsync(search, pagination);
        var result = mapper.Map<List<BookResponseDto>>(data);

        result = result.Zip(data, (dto, entity) =>
        {
            dto.BookId = entity.Id;
            return dto;
        }).ToList();

        return new GenericResponse<ICollection<BookResponseDto>>
        {
            Success = true,
            Message = "Libros obtenidos correctamente",
            Data = result
        };
    }

    public async Task<GenericResponse<BookResponseDto>> GetAsync(Guid id)
    {
        var data = await repository.GetAsync(id);
        if (data is null)
            throw new ResponseException($"No existe el libro con ID {id}", HttpStatusCode.NotFound);

        return new GenericResponse<BookResponseDto>
        {
            Success = true,
            Message = "Libro obtenido correctamente",
            Data = mapper.Map<BookResponseDto>(data)
        };
    }
    public async Task<GenericResponse<Guid>> AddAsync(BookRequestDto request)
    {
        var entity = mapper.Map<Book>(request);
        try
        {
            var id = await repository.AddAsync(entity);

            return new GenericResponse<Guid>
            {
                Success = true,
                Message = "Libro agregado correctamente",
                Data = id
            };
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException?.Message.Contains("isbn", StringComparison.OrdinalIgnoreCase) == true ||
                ex.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true)
            {
                throw new ResponseException("Ya existe un libro registrado con ese ISBN.", HttpStatusCode.BadRequest);
            }

            logger.LogError(ex, "Error al insertar libro");
            throw;
        }
    }
    public async Task<BaseGenericResponse> UpdateAsync(Guid id, BookUpdateRequestDto request)
    {
        var data = await repository.GetAsync(id);
        if (data is null)
            throw new ResponseException($"No existe el libro con ID {id}", HttpStatusCode.NotFound);
        mapper.Map(request, data);

        try
        {
            await repository.UpdateAsync(data);

            return new BaseGenericResponse
            {
                Success = true,
                Message = "Libro actualizado correctamente"
            };
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException?.Message.Contains("isbn", StringComparison.OrdinalIgnoreCase) == true ||
                ex.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true)
            {
                throw new ResponseException("Ya existe otro libro con ese ISBN.", HttpStatusCode.BadRequest);
            }

            logger.LogError(ex, "Error al actualizar libro");
            throw;
        }
    }

    public async Task<BaseGenericResponse> DeleteAsync(Guid id)
    {
        var data = await repository.GetAsync(id);
        if (data is null)
            throw new ResponseException($"No existe el libro con ID {id}", HttpStatusCode.NotFound);

        await repository.DeleteAsync(data);

        return new BaseGenericResponse
        {
            Success = true,
            Message = "Libro eliminado correctamente"
        };
    }


}