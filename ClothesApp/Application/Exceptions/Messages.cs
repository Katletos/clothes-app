namespace Application.Exceptions;

public class Messages
{
    public const string NotFound = "Item not found";

    public const string BrandDeleteConstraint = "You can't delete a brand that has one or more products";

    public const string BrandNameUniqueConstraint = "Brand name must be unique";

    public const string ReviewUniqueConstraint = "User already has review on this product";
    
    public const string EmailUniqueConstraint = "User with this email already exists";
    
    public const string CategoryUniqueConstraint = "Category with this name already exists";
    
    public const string CategoryDeleteConstraint = "You can't delete a category that is a parent category";
   
    public const string ProductNameUniqueConstraint = "Product with this name already exists";
}