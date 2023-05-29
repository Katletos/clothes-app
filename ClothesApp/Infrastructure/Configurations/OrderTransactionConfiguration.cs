using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class OrderTransactionConfiguration : IEntityTypeConfiguration<OrdersTransaction>
{
    public void Configure(EntityTypeBuilder<OrdersTransaction> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id);
        builder.Property(e => e.OrderId).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired()
            .HasColumnType("timestamp without time zone");
        
        builder.Property(e => e.OrderStatus).IsRequired();

        builder.HasOne(d => d.Order)
            .WithMany(p => p.OrdersTransactions)
            .HasForeignKey(d => d.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}