using AutoMapper;
using ClothesApp.Data.Dtos;
using ClothesApp.Entities;

namespace ClothesApp.Data;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {			
        CreateMap<Brand, BrandDto>().ReverseMap();
        CreateMap<CreateBrandDto, Brand>();
    }
}