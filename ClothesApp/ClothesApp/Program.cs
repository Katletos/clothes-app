using ClothesApp;
using ClothesApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;


using (var db = new ClothesAppContext())
{
    //
    
    Console.WriteLine("get_products_by_brand_id");
    var products = db.Products.Where(p => p.BrandId == 1).ToList();
    
    foreach (var p in products)
    {
        Console.WriteLine($"id: {p.Id}, " +
                          $"brand_id: {p.BrandId}, " +
                          $"category_id: {p.CategoryId}," +
                          $"name: {p.Name}," +
                          $"price: {p.Price}," +
                          $"quantity: {p.Quantity}," +
                          $"created at: {p.CreatedAt}");
    }
    
    //
    
    Console.WriteLine("get_brands_with_products_amount");
    
    //
    
    
    Console.WriteLine("get_products_by_section_and_category");
    
    //
    
    Console.WriteLine("get_completed_orders_by_product_id");
    
    //
    
    Console.WriteLine("get_all_reviews");
  
    //
}