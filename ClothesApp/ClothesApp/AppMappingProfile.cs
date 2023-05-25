using AutoMapper;
using ClothesApp.Dtos;
using ClothesApp.Entities;

namespace ClothesApp;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {			
        CreateMap<Brand, BrandDto>().ReverseMap();
        CreateMap<CreateBrandDto, Brand>();
    }
}