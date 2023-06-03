using Application.Dtos.Brands;
using Application.Dtos.Reviews;
using AutoMapper;
using Domain.Entities;

namespace Application;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<Brand, BrandDto>().ReverseMap();
        CreateMap<BrandInputDto, Brand>();
        CreateMap<Review, ReviewDto>().ReverseMap();
        CreateMap<ReviewInputDto, Review>();
    }
}