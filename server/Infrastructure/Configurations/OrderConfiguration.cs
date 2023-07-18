using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id);
        builder.Property(e => e.AddressId).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired()
            .HasColumnType("timestamp without time zone");
        builder.Property(e => e.Price).IsRequired()
            .HasColumnType("money");
        builder.Property(e => e.UserId).IsRequired();
        builder.Property(e => e.OrderStatus).IsRequired();

        builder.HasOne(d => d.Address)
            .WithMany(p => p.Orders)
            .HasForeignKey(d => d.AddressId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(d => d.User)
            .WithMany(p => p.Orders)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}