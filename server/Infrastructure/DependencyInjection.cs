using Application.Interfaces.Repositories;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ClothesAppContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IProductsRepository, ProductsRepository>();
        services.AddScoped<IBrandsRepository, BrandsRepository>();
        services.AddScoped<IReviewsRepository, ReviewsRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IOrderTransactionsRepository, OrderTransactionRepository>();
        services.AddScoped<IOrderItemsRepository, OrderItemsRepository>();
        services.AddScoped<ISectionRepository, SectionRepository>();
        services.AddScoped<ISectionCategoryRepository, SectionCategoryRepository>();
        services.AddScoped<IMediaRepository, MediaRepository>();
        services.AddScoped<ICartItemsRepository, CartItemsRepository>();

        return services;
    }
}