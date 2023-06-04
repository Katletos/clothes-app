using Application.Dtos.Brands;
using Application.Dtos.Products;
using Application.Dtos.Reviews;
using Application.Dtos.Users;
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
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<UserInputDto, User>();
        CreateMap<Order, OrderTransaction>();
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<ProductInputDto, Product>();
    }
}