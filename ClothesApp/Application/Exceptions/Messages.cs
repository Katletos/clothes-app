namespace Application.Exceptions;

public class Messages
{
    public const string NotFound = "Item not found";

    public const string BrandDeleteConstraint = "You can't delete a brand that has one or more products";

    public const string BrandNameUniqueConstraint = "Brand name must be unique";
}