using Application.Dtos.Addresses;
using Application.Dtos.Brands;
using Application.Dtos.Category;
using Application.Dtos.OrderItems;
using Application.Dtos.Orders;
using Application.Dtos.OrderTransactions;
using Application.Dtos.Products;
using Application.Dtos.Reviews;
using Application.Dtos.SectionCategories;
using Application.Dtos.Sections;
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
        CreateMap<UserInputInfoDto, User>();
        CreateMap<Order, OrderTransaction>();
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<ProductInputDto, Product>();
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<OrderInputDto, Order>();
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        CreateMap<OrderItemInputDto, OrderItem>();
        CreateMap<OrderItemInputDto, OrderItemDto>();
        CreateMap<OrderTransaction, OrderTransactionsDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<CategoryInputDto, Category>();
        CreateMap<Section, SectionDto>().ReverseMap();
        CreateMap<SectionInputDto, Section>();
        CreateMap<Address, AddressDto>().ReverseMap();
        
        CreateMap<SectionCategory, SectionCategoryDto>().ReverseMap();
    }
}