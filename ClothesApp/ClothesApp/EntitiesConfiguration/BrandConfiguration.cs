using ClothesApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesApp.EntitiesConfiguration;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable("brands");

        builder.HasIndex(e => e.Name).IsUnique();

        builder.Property(e => e.Id);
        builder.Property(e => e.Name).IsRequired();
    }
}