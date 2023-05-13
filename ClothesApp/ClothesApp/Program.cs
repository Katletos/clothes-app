using System.Collections;
using ClothesApp;
using ClothesApp.Entities;
using Microsoft.EntityFrameworkCore;

using var db = new ClothesAppContext();

Console.WriteLine("-----------------LAZY-----------------------");
Console.WriteLine("get_products_by_brand_id");
var products = await db.Products
    .Where(p => p.BrandId == 1)
    .ToListAsync();
    
Console.WriteLine("after query string");

foreach (var p in products)
{
    Console.WriteLine($"id: {p.Id}, " +
                      $"brand_id: {p.BrandId}, " +
                      $"brand_name: {p.Brand.Name}, " +
                      $"category_id: {p.CategoryId}," +
                      $"name: {p.Name}," +
                      $"price: {p.Price}," +
                      $"quantity: {p.Quantity}," +
                      $"created at: {p.CreatedAt}");
}
    
Console.WriteLine("---------------------------------------------------------");
    
Console.WriteLine("get_brands_with_products_amount");
var brands = await db.Products
    .GroupBy(b => b.BrandId, (b, p) => 
        new
        {
            BrandId = b,
            ProductCount = p.Count()
        })
    .OrderByDescending(p => p.ProductCount)
    .ToListAsync();

Console.WriteLine("after query string");
    
foreach(var b in brands)
{
    Console.WriteLine($"brand_id: {b.BrandId}, " +
                      $"product_count: {b.ProductCount}");
}

Console.WriteLine("---------------------------------------------------------");
    
Console.WriteLine("get_products_by_section_and_category");
var productsBySectionCategory = await db.Products
    .Where(p => p.Category.Id == 2 && p.Category.Sections.Any(s => s.Id == 1))
    .ToListAsync();
     
Console.WriteLine("after query string");

foreach (var p in productsBySectionCategory)
{
    Console.WriteLine($"id: {p.Id}, " +
                      $"brand_id: {p.BrandId}, " +
                      $"brand_name: {p.Brand.Name}, " +
                      $"category_id: {p.CategoryId}," +
                      $"category_name: {p.Category.Name}," +
                      $"name: {p.Name}," +
                      $"price: {p.Price}," +
                      $"quantity: {p.Quantity}," +
                      $"created at: {p.CreatedAt}");
}
     
Console.WriteLine("---------------------------------------------------------");
    
Console.WriteLine("get_completed_orders_by_product_id");
var orders = await db.Orders
    .Where(o => o.OrderStatus == OrderStatusType.Completed && o.OrdersItems.Any(p => p.ProductId == 1))
    .OrderByDescending(o => o.CreatedAt)
    .ToListAsync();

Console.WriteLine("after query string");

foreach (var o in orders)
{
    Console.WriteLine($"id: {o.Id}, " +
                      $"user_name: {o.User.FirstName + " " + o.User.LastName}, " +
                      $"price: {o.Price}, " +
                      $"address_id: {o.Address.AddressLine}, " +
                      $"created_at: {o.CreatedAt}, " +
                      $"status: {o.OrderStatus}");
}
    
Console.WriteLine("---------------------------------------------------------");
    
Console.WriteLine("get_all_reviews");
var reviews = await db.Reviews
    .Where(r => r.ProductId == 1 && r.User.Id == r.UserId)
    .ToListAsync();

Console.WriteLine("after query string");

foreach (var r in reviews)
{
    Console.WriteLine($"rating: {r.Rating}," +
                      $"title: {r.Title}," +
                      $"comment: {r.Comment}," +
                      $"name: {r.User.FirstName + " " + r.User.LastName}");
}

Console.WriteLine("-----------------------------EAGER-------------------------------------");

Console.WriteLine("get_products_by_brand_id");
var productse = db.Products
    .Include(b => b.Brand)
    .Where(p => p.BrandId == 1);

Console.WriteLine("after query string");

foreach (var p in productse)
{
    Console.WriteLine($"id: {p.Id}, " +
                      $"brand_id: {p.BrandId}, " +
                      $"brand_name: {p.Brand.Name}, " +
                      $"category_id: {p.CategoryId}," +
                      $"name: {p.Name}," +
                      $"price: {p.Price}," +
                      $"quantity: {p.Quantity}," +
                      $"created at: {p.CreatedAt}");
}
    
Console.WriteLine("---------------------------------------------------------");
    
Console.WriteLine("get_brands_with_products_amount");
var brandse = db.Products
    .GroupBy(b => b.BrandId, (b, p) => 
        new
        {
            BrandId = b,
            ProductCount = p.Count()
        })
    .OrderByDescending(p => p.ProductCount);
    

Console.WriteLine("after query string");

foreach(var b in brandse)
{
    Console.WriteLine($"brand_id: {b.BrandId}, " +
                      $"product_count: {b.ProductCount}");
}

Console.WriteLine("---------------------------------------------------------");
    
Console.WriteLine("get_products_by_section_and_category");
var productsBySectionCategorye = db.Products
    .Include(b => b.Brand)
    .Include(c => c.Category)
    .Include(s => s.Category.Sections)
    .Include(sc => sc.Category.SectionCategories)
    .Where(p => p.Category.Id == 2 && p.Category.Sections.Any(s => s.Id == 1));
    

Console.WriteLine("after query string");

foreach (var p in productsBySectionCategorye)
{
    Console.WriteLine($"id: {p.Id}, " +
                      $"brand_id: {p.BrandId}, " +
                      $"brand_name: {p.Brand.Name}, " +
                      $"category_id: {p.CategoryId}," +
                      $"category_name: {p.Category.Name}," +
                      $"name: {p.Name}," +
                      $"price: {p.Price}," +
                      $"quantity: {p.Quantity}," +
                      $"created at: {p.CreatedAt}");
}
     
Console.WriteLine("---------------------------------------------------------");
    
Console.WriteLine("get_completed_orders_by_product_id");
var orderse = db.Orders
    .Include(o => o.OrdersItems)
    .Include(a => a.Address)
    .Include(u => u.User)
    .Where(o => o.OrderStatus == OrderStatusType.Completed && o.OrdersItems.Any(p => p.ProductId == 1))
    .OrderByDescending(o => o.CreatedAt);

Console.WriteLine("after query string");

foreach (var o in orderse)
{
    Console.WriteLine($"id: {o.Id}, " +
                      $"user_name: {o.User.FirstName + " " + o.User.LastName}, " +
                      $"price: {o.Price}, " +
                      $"address_id: {o.Address.AddressLine}, " +
                      $"created_at: {o.CreatedAt}, " +
                      $"status: {o.OrderStatus}");
}
    
Console.WriteLine("---------------------------------------------------------");
    
Console.WriteLine("get_all_reviews");
var reviewse = db.Reviews
    .Include(u => u.User)
    .Where(r => r.ProductId == 1 && r.User.Id == r.UserId);

Console.WriteLine("after query string");

foreach (var r in reviewse)
{
    Console.WriteLine($"rating: {r.Rating}," +
                      $"title: {r.Title}," +
                      $"comment: {r.Comment}," +
                      $"name: {r.User.FirstName + " " + r.User.LastName}");
}

var brand = new Brand(){Name = "mark formelle" };

using (var context = new ClothesAppContext())
{
    context.Add<Brand>(brand);
    context.SaveChanges();
    foreach (var tmp in context.Brands)
    {
        Console.WriteLine($"brand id: {tmp.Id}, name: {tmp.Name}");   
    }
}