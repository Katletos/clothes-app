using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ClothesApp;

public class SampleContextFactory : IDesignTimeDbContextFactory<ClothesAppContext>
 {
     public ClothesAppContext CreateDbContext(string[] args)
     {
         var optionsBuilder = new DbContextOptionsBuilder<ClothesAppContext>();
         
         ConfigurationBuilder builder = new ConfigurationBuilder();
         builder.SetBasePath(Directory.GetCurrentDirectory());
         builder.AddJsonFile("appsettings.json");
         IConfigurationRoot config = builder.Build();
  
         string connectionString = config.GetConnectionString("DefaultConnection");
         optionsBuilder.UseNpgsql(connectionString);
         return new ClothesAppContext(optionsBuilder.Options);
     }
 }