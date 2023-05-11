using ClothesApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesApp.EntitiesConfiguration;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.Name).IsUnique();

        builder.Property(e => e.Id);
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.ParentCategoryId);

        builder.HasOne(d => d.ParentCategory)
            .WithMany(p => p.ChildCategory)
            .HasForeignKey(d => d.ParentCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Sections)
            .WithMany(p => p.Categories)
            .UsingEntity<SectionCategory>(
                l => l.HasOne<Section>().WithMany(e => e.SectionCategories),
                r => r.HasOne<Category>().WithMany(e => e.SectionCategories)
            );
    }
}