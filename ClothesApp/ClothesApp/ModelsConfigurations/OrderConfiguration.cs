using ClothesApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesApp.ModelsConfigurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.Id).HasName("orders_pkey");

        builder.ToTable("orders");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.AddressId).HasColumnName("address_id");
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("created_at");
        builder.Property(e => e.Price)
            .HasColumnType("money")
            .HasColumnName("price");
        builder.Property(e => e.UserId).HasColumnName("user_id");

        builder.HasOne(d => d.Address).WithMany(p => p.Orders)
            .HasForeignKey(d => d.AddressId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("orders_address_id_fkey");

        builder.HasOne(d => d.User).WithMany(p => p.Orders)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("orders_user_id_fkey");
    }
}