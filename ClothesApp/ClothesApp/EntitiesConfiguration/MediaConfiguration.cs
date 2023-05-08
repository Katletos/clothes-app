using ClothesApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesApp.EntitiesConfiguration;

public class MediaConfiguration : IEntityTypeConfiguration<Media>
{
    public void Configure(EntityTypeBuilder<Media> builder)
    {
        builder.HasKey(e => e.Id).HasName("media_pkey");

        builder.ToTable("media");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.FileName).HasColumnName("file_name").IsRequired();
        builder.Property(e => e.FileType).HasColumnName("file_type").IsRequired();
        builder.Property(e => e.ProductId).HasColumnName("product_id").IsRequired();
        builder.Property(e => e.Url).HasColumnName("url").IsRequired();

        builder.HasOne(d => d.Product).WithMany(p => p.Media)
            .HasForeignKey(d => d.ProductId)
            .HasConstraintName("media_product_id_fkey");
    }
}