using ClothesApp;
using ClothesApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory());
builder.AddJsonFile("appsettings.json");
var config = builder.Build();

string? connectionString = config.GetConnectionString("DefaultConnection");
 
var optionsBuilder = new DbContextOptionsBuilder<ClothesAppContext>();
var options = optionsBuilder.UseNpgsql(connectionString).Options;

using (ClothesAppContext db = new ClothesAppContext(options))
{
    bool isAvalaible = await db.Database.CanConnectAsync();
    Console.WriteLine(isAvalaible ? "DB available" : "DB not available");
}