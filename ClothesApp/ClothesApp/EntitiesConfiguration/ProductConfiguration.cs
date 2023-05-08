using ClothesApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesApp.EntitiesConfiguration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(e => e.Id).HasName("products_pkey");

        builder.ToTable("products");

        builder.HasIndex(e => e.Name, "products_name_key").IsUnique();

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.BrandId).HasColumnName("brand_id").IsRequired();
        builder.Property(e => e.CategoryId).HasColumnName("category_id").IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired()
            .HasColumnType("timestamp without time zone")
            .HasColumnName("created_at");
        builder.Property(e => e.Name).HasColumnName("name").IsRequired();
        builder.Property(e => e.Price).IsRequired()
            .HasColumnType("money")
            .HasColumnName("price");
        builder.Property(e => e.Quantity).HasColumnName("quantity").IsRequired();

        builder.HasOne(d => d.Brand).WithMany(p => p.Products)
            .HasForeignKey(d => d.BrandId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("products_brand_id_fkey");

        builder.HasOne(d => d.Category).WithMany(p => p.Products)
            .HasForeignKey(d => d.CategoryId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("products_category_id_fkey");
    }
}