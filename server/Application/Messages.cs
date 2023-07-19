namespace Application;

public class Messages
{
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

    public const string BrandNotFound = "Brand not found";

    public const string AddressNotFound = "Address not found";

    public const string CategoryNotFound = "Category not foun";

    public const string SelfReferencingCategory = "Self referencing category";

    public const string SectionNotFound = "Section not found";

    public const string OrderNotFound = "Order not found";

    public const string UserNotFound = "User not found";

    public const string ReviewNotFound = "Review not found";

    public const string ProductNotFound = "Product not found";

    public const string SectionCategoryRelation = "Section and category relation already exist";

    public const string AddressUserConstraint = "Address don't belongs user";

    public const string OrderUpdateConstraint = "Order in the final state";

    public const string OrderTransitionConstraint = "Incorrect transition";

    public const string FileUploadConstraint = "File size is more than 50MB";

    public const string EmptyFile = "File is empty";

    public const string ReviewAddConstraint = "The user did not purchase this product";

    public const string InvalidPassword = "Invalid password";

    public const string AuthorizationConstraint = "You are not authorised to access this resource";

    public const string CartItemNotFound = "Cart item not found";

    public const string NotEnoughQuantity = "Don't enough quantity";

    public const string CartItemAlreadyExist = "Cart item already exist";

    public const string MoreThanInStockQuantity = "You can’t submit an order with quantity more than in stock quantity";
}