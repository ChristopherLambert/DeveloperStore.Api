using AutoMapper;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Services.DTOs;

public class SaleProfile : Profile
{
    public SaleProfile()
    {
        CreateMap<Sale, SaleDto>().ReverseMap();
        CreateMap<SaleItem, SaleItemDto>().ReverseMap();

        CreateMap<SaleItem, ProductDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => $"Generated description for {src.ProductName}"))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => "default-category"))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => "https://fakestoreapi.com/img/81fPKd-2AYL._AC_SL1500_.jpg"))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new RatingDto
            {
                Rate = 4.0,
                Count = 100
            }));
    }
}
