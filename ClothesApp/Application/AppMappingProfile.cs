using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<Brand, BrandDto>().ReverseMap();
        CreateMap<BrandNameDto, Brand>();
    }
}