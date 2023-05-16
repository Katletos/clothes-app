using ClothesApp;
using ClothesApp.Entities;
using Microsoft.EntityFrameworkCore;

await using var db = new ClothesAppContext();

var productsByBrandIdLazy = await GetProductsByBrandIdLazy(db, 1);
await GetBrandsWithProductAmountLazy(db);
var productsBySectionCategoryLazy = await GetProductsBySectionCategoryLazy(db, 1, 1);
var completedOrdersByProductIdLazy = await GetCompletedOrdersByProductIdLazy(db, 1);
var reviewsForProductLazy = await GetAllReviewsForProductLazy(db, 1);

var productsByBrandId = await GetProductsByBrandId(db, 1);
await GetBrandsWithProductsAmount(db);
var productsBySectionCategory = await GetProductsBySectionCategory(db, 1, 1);
var completedOrdersByProductId = await GetCompletedOrdersByProductId(db, 1);
var reviewsForProduct = await GetAllReviewsForProduct(db, 1);

var brand = await db.Brands.FindAsync((long)4);
brand.Name = "mark formelle";

await using (var context = new ClothesAppContext())
{
    context.Update(brand);
    await context.SaveChangesAsync();
}

async Task<List<Product>> GetProductsByBrandIdLazy(ClothesAppContext context, long brandId)
{
    var result = await context.Products
        .Where(p => p.BrandId == brandId)
        .ToListAsync();
    
    foreach (var r in result)
    {
        r.Brand.Name = "something";
    }

    return result;
}

async Task GetBrandsWithProductAmountLazy(ClothesAppContext context)
{
    var products = await context.Products.ToListAsync();
    
    foreach (var r in products)
    {
        r.Category.Name = "something";
    }
    
    var result = products.GroupBy(b => b.BrandId, (b, p) =>
            new
            {
                BrandId = b,
                ProductCount = p.Count()
            })
        .OrderByDescending(p => p.ProductCount);
}

async Task<List<Product>> GetProductsBySectionCategoryLazy(ClothesAppContext context, long sectionId, long categoryId)
{
    var result = await context.Products
        .Where(p => p.Category.Id == categoryId 
                    && p.Category.Sections.Any(s => s.Id == sectionId))
        .ToListAsync();
    
    foreach (var r in result)
    {
        r.Brand.Name = "something";
    }

    return result;
}

async Task<List<Order>> GetCompletedOrdersByProductIdLazy(ClothesAppContext context, long productId)
{
    var result = await context.Orders
        .Where(o => o.OrderStatus == OrderStatusType.Completed 
                    && o.OrdersItems.Any(p => p.ProductId == productId))
        .OrderByDescending(o => o.CreatedAt)
        .ToListAsync();
    
    foreach (var r in result)
    {
        r.User.FirstName = "something";
    }
    
    return result;
}

async Task<List<Review>> GetAllReviewsForProductLazy(ClothesAppContext context, long productId)
{
    var result = await context.Reviews
        .Where(r => r.ProductId == productId && r.User.Id == r.UserId)
        .ToListAsync();
    
    foreach (var r in result)
    {
        r.User.FirstName = "something";
    }
    
    return result;
}
    
async Task<List<Product>> GetProductsByBrandId(ClothesAppContext context, long brandId)
{
    var result = await context.Products
        .Include(b => b.Brand)
        .Where(p => p.BrandId == brandId)
        .ToListAsync();
    
    return result;
}

async Task GetBrandsWithProductsAmount(ClothesAppContext context)
{
    var result = await context.Products
        .GroupBy(b => b.BrandId, (b, p) =>
            new
            {
                BrandId = b,
                ProductCount = p.Count(),
            })
        .OrderByDescending(p => p.ProductCount)
        .ToListAsync();
}

async Task<List<Product>> GetProductsBySectionCategory(ClothesAppContext context, long sectionId, long categoryId)
{
    var result = await context.Products
        .Include(c => c.Category)
        .ThenInclude(c => c.Sections)
        .ThenInclude(c => c.SectionCategories)
        .Where(p => p.Category.Id == categoryId && p.Category.Sections.Any(s => s.Id == sectionId))
        .ToListAsync(); 
    
    return result;
}

async Task<List<Order>> GetCompletedOrdersByProductId(ClothesAppContext context, long productId)
{
    var result = await context.Orders
        .Include(o => o.OrdersItems)
        .Where(o => o.OrderStatus == OrderStatusType.Completed 
                    && o.OrdersItems.Any(p => p.ProductId == productId))
        .OrderByDescending(o => o.CreatedAt)
        .ToListAsync(); 
    
    return result;
}

async Task<List<Review>> GetAllReviewsForProduct(ClothesAppContext context, long productId)
{
    var result = await context.Reviews
        .Include(u => u.User)
        .Where(r => r.ProductId == productId && r.User.Id == r.UserId)
        .ToListAsync();

    return result;
}