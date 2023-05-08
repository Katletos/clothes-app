using ClothesApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesApp.EntitiesConfiguration;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(e => e.Id).HasName("reviews_pkey");

        builder.ToTable("reviews");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Comment).HasColumnName("comment").IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired()
            .HasColumnType("timestamp without time zone")
            .HasColumnName("created_at");
        builder.Property(e => e.ProductId).HasColumnName("product_id").IsRequired();
        builder.Property(e => e.Rating).HasColumnName("rating").IsRequired();
        builder.Property(e => e.Title).HasColumnName("title").IsRequired();
        builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();

        builder.HasOne(d => d.Product).WithMany(p => p.Reviews)
            .HasForeignKey(d => d.ProductId)
            .HasConstraintName("reviews_product_id_fkey");

        builder.HasOne(d => d.User).WithMany(p => p.Reviews)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("reviews_user_id_fkey");
    }
}