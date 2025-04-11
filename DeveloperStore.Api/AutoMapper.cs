using AutoMapper;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Services.DTOs;

public class SaleProfile : Profile
{
    public SaleProfile()
    {
        CreateMap<Sale, SaleDto>().ReverseMap();
        CreateMap<SaleItem, SaleItemDto>().ReverseMap();
    }
}
