using ClothesApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesApp.EntitiesConfiguration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.Id).HasName("orders_pkey");

        builder.ToTable("orders");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.AddressId).HasColumnName("address_id").IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired()
            .HasColumnType("timestamp without time zone")
            .HasColumnName("created_at");
        builder.Property(e => e.Price).IsRequired()
            .HasColumnType("money")
            .HasColumnName("price");
        builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(e => e.OrderStatus).HasColumnName("order_status").IsRequired();

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