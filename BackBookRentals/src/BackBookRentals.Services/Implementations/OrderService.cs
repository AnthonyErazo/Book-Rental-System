using AutoMapper;
using BackBookRentals.Dto.Request;
using BackBookRentals.Dto.Response;
using BackBookRentals.Entities;
using BackBookRentals.Repositories.Abstractions;
using BackBookRentals.Services.Abstractions;

namespace BackBookRentals.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly IOrderRepository repository;
    private readonly IMapper mapper;

    public OrderService(IOrderRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<GenericResponse<ICollection<OrderResponseDto>>> GetAsync(PaginationDto pagination)
    {
        var data = await repository.GetAsync(pagination);
        var result = mapper.Map<List<OrderResponseDto>>(data);

        return new GenericResponse<ICollection<OrderResponseDto>>
        {
            Success = true,
            Message = "Libros obtenidos correctamente",
            Data = result
        };
    }

    public async Task<GenericResponse<Guid>> CreateAsync(OrderRequestDto request)
    {
        var order = new Order
        {
            ClientId = request.ClientId,
            Status = true,
            OrderBooks = request.BookIds.Select(bookId => new OrderBook
            {
                BookId = bookId
            }).ToList()
        };

        var id = await repository.AddAsync(order);
        return new GenericResponse<Guid>
        {
            Success = true,
            Message = "Pedido registrado",
            Data = id
        };
    }

    public async Task<GenericResponse<ICollection<BookResponseDto>>> GetBooksByDniAsync(string dni)
    {
        var books = await repository.GetBooksByClientDniAsync(dni);
        return new GenericResponse<ICollection<BookResponseDto>>
        {
            Success = true,
            Message = "Libros encontrados",
            Data = mapper.Map<ICollection<BookResponseDto>>(books)
        };
    }
    public async Task<GenericResponse<ICollection<OrderByClientResponseDto>>> GetOrdersByClientIdAsync(Guid clientId)
    {
        var orders = await repository.GetOrdersByClientIdAsync(clientId);
        var result = mapper.Map<ICollection<OrderByClientResponseDto>>(orders);
        return new GenericResponse<ICollection<OrderByClientResponseDto>>
        {
            Success = true,
            Message = "Pedidos del cliente encontrados",
            Data = result
        };
    }

    public async Task<GenericResponse<ICollection<OrderByBookResponseDto>>> GetOrdersByBookIdAsync(Guid bookId)
    {
        var orders = await repository.GetOrdersByBookIdAsync(bookId);
        var result = mapper.Map<ICollection<OrderByBookResponseDto>>(orders);
        return new GenericResponse<ICollection<OrderByBookResponseDto>>
        {
            Success = true,
            Message = "Pedidos asociados al libro encontrados",
            Data = result
        };
    }

    public async Task<BaseGenericResponse> UpdateStatusAsync(Guid orderId)
    {
        var order = await repository.GetAsync(orderId);
        if (order is null)
            return new BaseGenericResponse { Success = false, Message = "Pedido no encontrado" };

        order.Status = !order.Status;
        await repository.UpdateAsync(order);

        return new BaseGenericResponse { Success = true, Message = "Estado actualizado" };
    }

    public async Task<BaseGenericResponse> DeleteAsync(Guid orderId)
    {
        var order = await repository.GetAsync(orderId);
        if (order is null)
            return new BaseGenericResponse { Success = false, Message = "Pedido no encontrado" };

        await repository.DeleteAsync(order);
        return new BaseGenericResponse { Success = true, Message = "Pedido eliminado" };
    }
}
