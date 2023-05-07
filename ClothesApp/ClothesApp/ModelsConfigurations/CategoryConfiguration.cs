using ClothesApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ClothesApp.ModelsConfigurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(e => e.Id).HasName("categories_pkey");

        builder.ToTable("categories");

        builder.HasIndex(e => e.Name, "categories_name_key").IsUnique();

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.ParentCategoryId).HasColumnName("parent_category_id");

        builder.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
            .HasForeignKey(d => d.ParentCategoryId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("categories_parent_category_id_fkey");

        builder.HasMany(d => d.Sections).WithMany(p => p.Categories)
            .UsingEntity<Dictionary<string, object>>(
                "SectionsCategory",
                r => r.HasOne<Section>().WithMany()
                    .HasForeignKey("SectionId")
                    .HasConstraintName("sections_categories_section_id_fkey"),
                l => l.HasOne<Category>().WithMany()
                    .HasForeignKey("CategoryId")
                    .HasConstraintName("sections_categories_category_id_fkey"),
                j =>
                {
                    j.HasKey("CategoryId", "SectionId").HasName("sections_categories_pkey");
                    j.ToTable("sections_categories");
                    j.IndexerProperty<long>("CategoryId").HasColumnName("category_id");
                    j.IndexerProperty<long>("SectionId").HasColumnName("section_id");
                });
    }
}