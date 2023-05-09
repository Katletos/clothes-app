using ClothesApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesApp.EntitiesConfiguration;

public class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable("sections");

        builder.HasIndex(e => e.Name).IsUnique();

        builder.Property(e => e.Id);
        builder.Property(e => e.Name).IsRequired();
    }
}