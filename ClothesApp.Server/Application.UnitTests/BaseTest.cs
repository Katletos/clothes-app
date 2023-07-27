using Application;
using AutoMapper;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace UnitTests;

public class BaseTest
{
    protected readonly ClothesAppContext Context = ContextGenerator();

    protected readonly IMapper Mapper = new Mapper(new MapperConfiguration(cfg
        => cfg.AddProfile<AppMappingProfile>()));

    private static ClothesAppContext ContextGenerator()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ClothesAppContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var context = new ClothesAppContext(optionsBuilder.Options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }
}