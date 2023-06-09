namespace Application;

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
    
    public const string ProductAvailable = "Product available";
    
    public const string ProductOutOfStock = "Product is out of stock";
   
    public const string SectionUniqueConstraint = "Section name must be unique";

    public const string ParentCategoryConstraint = "No parental category exists";
    
    public const string SectionDeleteConstraint = "You can't delete a section that have related categories";

}