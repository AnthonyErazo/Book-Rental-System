using AutoMapper;
using BackBookRentals.Dto.Request;
using BackBookRentals.Dto.Response;
using BackBookRentals.Entities;

namespace BackBookRentals.Services.Profiles;

public class ClientProfile:Profile
{
    public ClientProfile()
    {
        CreateMap<ClientRequestDto, Client>();
        CreateMap<Client, ClientResponseDto>()
            .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.Id));
        CreateMap<ClientUpdateRequestDto, Client>()
            .ForMember(dest => dest.Age, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Orders, opt => opt.Ignore())
            .ForMember(dest => dest.Names, opt => opt.Condition((src, dest, srcMember) => src.Names != null))
            .ForMember(dest => dest.LastNames, opt => opt.Condition((src, dest, srcMember) => src.LastNames != null))
            .ForMember(dest => dest.Dni, opt => opt.Condition((src, dest, srcMember) => src.Dni != null));
    }
}