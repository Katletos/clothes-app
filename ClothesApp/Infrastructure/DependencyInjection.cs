using Application.Repositories;
using Application.Services;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ClothesAppContext>();
        
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBrandsRepository, BrandsRepository>();
        services.AddScoped<IBrandService, BrandService>();
        
        return services;
    }
}