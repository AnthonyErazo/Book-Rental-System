using AutoMapper;
using BackBookRentals.Dto.Request;
using BackBookRentals.Repositories.Abstractions;
using BackBookRentals.Services.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using BackBookRentals.Dto.Exception;
using BackBookRentals.Dto.Response;
using BackBookRentals.Entities;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace BackBookRentals.Services.Implementations;

public class ClientService : IClientsService
{
    private readonly IClientRepository repository;
    private readonly ILogger<ClientService> logger;
    private readonly IMapper mapper;

    public ClientService(
        IClientRepository repository,
        ILogger<ClientService> logger,
        IMapper mapper
        )
    {
        this.repository = repository;
        this.logger = logger;
        this.mapper = mapper;
    }
    public async Task<GenericResponse<ICollection<ClientResponseDto>>> GetAsync(string? search, PaginationDto pagination)
    {
        var data = await repository.GetAsync(search, pagination);
        return new GenericResponse<ICollection<ClientResponseDto>>
        {
            Success = true,
            Message = "Clientes obtenidos correctamente",
            Data = mapper.Map<ICollection<ClientResponseDto>>(data)
        };
    }

    public async Task<GenericResponse<ClientResponseDto>> GetAsync(Guid id)
    {
        var data = await repository.GetAsync(id);
        if (data is null)
            throw new ResponseException($"No existe el cliente con ID {id}", HttpStatusCode.NotFound);

        return new GenericResponse<ClientResponseDto>
        {
            Success = true,
            Message = "Cliente obtenido correctamente",
            Data = mapper.Map<ClientResponseDto>(data)
        };
    }
    public async Task<GenericResponse<Guid>> AddAsync(ClientRequestDto request)
    {
        var entity = mapper.Map<Client>(request);
        try
        {
            var id = await repository.AddAsync(entity);

            return new GenericResponse<Guid>
            {
                Success = true,
                Message = "Cliente agregado correctamente",
                Data = id
            };
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException?.Message.Contains("dni", StringComparison.OrdinalIgnoreCase) == true ||
                ex.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true)
            {
                throw new ResponseException("Ya existe un cliente registrado con ese DNI.", HttpStatusCode.BadRequest);
            }

            logger.LogError(ex, "Error al insertar cliente");
            throw;
        }
    }
    public async Task<BaseGenericResponse> UpdateAsync(Guid id, ClientUpdateRequestDto request)
    {
        var data = await repository.GetAsync(id);
        if (data is null)
            throw new ResponseException($"No existe el cliente con ID {id}", HttpStatusCode.NotFound);
        mapper.Map(request, data);
        if (request.Age.HasValue)
        {
            data.Age = request.Age.Value;
        }

        try
        {
            await repository.UpdateAsync(data);

            return new BaseGenericResponse
            {
                Success = true,
                Message = "Cliente actualizado correctamente"
            };
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException?.Message.Contains("dni", StringComparison.OrdinalIgnoreCase) == true ||
                ex.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true)
            {
                throw new ResponseException("Ya existe otro cliente con ese DNI.", HttpStatusCode.BadRequest);
            }

            logger.LogError(ex, "Error al actualizar cliente");
            throw;
        }
    }

    public async Task<BaseGenericResponse> DeleteAsync(Guid id)
    {
        var data = await repository.GetAsync(id);
        if (data is null)
            throw new ResponseException($"No existe el cliente con ID {id}", HttpStatusCode.NotFound);

        await repository.DeleteAsync(data);

        return new BaseGenericResponse
        {
            Success = true,
            Message = "Cliente eliminado correctamente"
        };
    }

    
}