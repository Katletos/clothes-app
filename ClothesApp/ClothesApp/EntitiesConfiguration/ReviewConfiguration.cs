using ClothesApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesApp.EntitiesConfiguration;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable("reviews");

        builder.Property(e => e.Id);
        builder.Property(e => e.Comment).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired()
            .HasColumnType("timestamp without time zone");
        builder.Property(e => e.ProductId).IsRequired();
        builder.Property(e => e.Rating).IsRequired();
        builder.Property(e => e.Title).IsRequired();
        builder.Property(e => e.UserId).IsRequired();

        builder.HasOne(d => d.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.User)
            .WithMany(p => p.Reviews)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}