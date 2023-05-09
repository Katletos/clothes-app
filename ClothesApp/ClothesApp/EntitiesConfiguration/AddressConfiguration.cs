using ClothesApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesApp.EntitiesConfiguration;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable("addresses");

        builder.Property(e => e.Id);
        builder.Property(e => e.AddressLine).IsRequired();
        builder.Property(e => e.UserId).IsRequired();

        builder.HasOne(d => d.User)
            .WithMany(p => p.Addresses)
            .HasForeignKey(d => d.UserId);
    }
}