using AutoMapper;
using BackBookRentals.Dto.Response;
using BackBookRentals.Entities;

namespace BackBookRentals.Services.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderResponseDto>()
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Client))
            .ForMember(dest => dest.Books, opt => opt.MapFrom(src =>
                src.OrderBooks.Select(ob => ob.Book)))
            .ReverseMap();
        CreateMap<Order, OrderByClientResponseDto>()
            .ForMember(dest => dest.Books, opt => opt.MapFrom(src =>
                src.OrderBooks.Select(ob => ob.Book)))
            .ReverseMap();
        CreateMap<Order, OrderByBookResponseDto>()
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Client))
            .ReverseMap();
        CreateMap<Client, ClientResponseDto>()
            .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.Id));
        CreateMap<Book, BookResponseDto>()
            .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Id));
    }
}