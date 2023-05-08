using ClothesApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesApp.EntitiesConfiguration;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(e => e.Id).HasName("categories_pkey");

        builder.ToTable("categories");

        builder.HasIndex(e => e.Name, "categories_name_key").IsUnique();

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Name).HasColumnName("name").IsRequired();
        builder.Property(e => e.ParentCategoryId).HasColumnName("parent_category_id");

        builder.HasOne(d => d.ParentCategory)
            .WithMany(p => p.ChildCategory)
            .HasForeignKey(d => d.ParentCategoryId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("categories_parent_category_id_fkey");

        builder.HasMany(d => d.Sections)
            .WithMany(p => p.Categories)
            .UsingEntity<SectionCategory>(
                l => l.HasOne<Section>().WithMany(e => e.SectionCategories),
                r => r.HasOne<Category>().WithMany(e => e.SectionCategories)
            );
    }
}