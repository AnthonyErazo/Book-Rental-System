using AutoMapper;
using BackBookRentals.Dto.Request;
using BackBookRentals.Dto.Response;
using BackBookRentals.Entities;

namespace BackBookRentals.Services.Profiles;

public class BookProfile: Profile
{
    public BookProfile()
    {
        CreateMap<BookRequestDto, Book>();
        CreateMap<Book, BookResponseDto>();
        CreateMap<BookUpdateRequestDto, Book>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Author, opt => opt.Condition((src, dest, srcMember) => src.Author != null))
            .ForMember(dest => dest.Name, opt => opt.Condition((src, dest, srcMember) => src.Name != null))
            .ForMember(dest => dest.Status, opt => opt.Condition((src, dest, srcMember) => src.Status != null))
            .ForMember(dest => dest.Isbn, opt => opt.Condition((src, dest, srcMember) => src.Isbn != null));
    }
}