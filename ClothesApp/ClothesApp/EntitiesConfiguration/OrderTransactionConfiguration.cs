using ClothesApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesApp.EntitiesConfiguration;

public class OrderTransactionConfiguration : IEntityTypeConfiguration<OrdersTransaction>
{
    public void Configure(EntityTypeBuilder<OrdersTransaction> builder)
    {
        builder.HasKey(e => e.Id).HasName("orders_transactions_pkey");

        builder.ToTable("orders_transactions");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.OrderId).HasColumnName("order_id").IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired()
            .HasColumnType("timestamp without time zone")
            .HasColumnName("updated_at");
        builder.Property(e => e.OrderStatus).HasColumnName("order_status").IsRequired();

        builder.HasOne(d => d.Order).WithMany(p => p.OrdersTransactions)
            .HasForeignKey(d => d.OrderId)
            .HasConstraintName("orders_transactions_order_id_fkey");
    }
}