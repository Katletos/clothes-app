using Application.Repositories;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ClothesAppContext>();

        services.AddScoped<IProductsRepository, ProductsRepository>();
        services.AddScoped<IBrandsRepository, BrandsRepository>();
      
        return services;
    }
}