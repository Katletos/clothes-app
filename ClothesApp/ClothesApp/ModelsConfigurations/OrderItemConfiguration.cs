using ClothesApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesApp.ModelsConfigurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(e => new {e.ProductId, e.OrderId }).HasName("orders_items_pkey");

        builder.ToTable("orders_items");

        builder.Property(e => e.ProductId).HasColumnName("product_id");
        builder.Property(e => e.OrderId).HasColumnName("order_id");
        builder.Property(e => e.Price)
            .HasColumnType("money")
            .HasColumnName("price");
        builder.Property(e => e.Quantity).HasColumnName("quantity");

        builder.HasOne(d => d.Order).WithMany(p => p.OrdersItems)
            .HasForeignKey(d => d.OrderId)
            .HasConstraintName("orders_items_order_id_fkey");

        builder.HasOne(d => d.Product).WithMany(p => p.OrdersItems)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("orders_items_product_id_fkey");
    }
}