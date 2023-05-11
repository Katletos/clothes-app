using ClothesApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesApp.EntitiesConfiguration;

public class MediaConfiguration : IEntityTypeConfiguration<Media>
{
    public void Configure(EntityTypeBuilder<Media> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id);
        builder.Property(e => e.FileName).IsRequired();
        builder.Property(e => e.FileType).IsRequired();
        builder.Property(e => e.ProductId).IsRequired();
        builder.Property(e => e.Url).IsRequired();

        builder.HasOne(d => d.Product)
            .WithMany(p => p.Media)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}