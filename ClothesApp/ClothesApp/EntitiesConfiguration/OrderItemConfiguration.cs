using ClothesApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesApp.EntitiesConfiguration;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(e => new {e.ProductId, e.OrderId });

        builder.Property(e => e.ProductId);
        builder.Property(e => e.OrderId);
        builder.Property(e => e.Price)
            .HasColumnType("money");
        builder.Property(e => e.Quantity);

        builder.HasOne(d => d.Order)
            .WithMany(p => p.OrdersItems)
            .HasForeignKey(d => d.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.Product)
            .WithMany(p => p.OrdersItems)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}