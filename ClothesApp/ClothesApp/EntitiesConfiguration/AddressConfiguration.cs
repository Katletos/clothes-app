using ClothesApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesApp.EntitiesConfiguration;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(e => e.Id).HasName("addresses_pkey");

        builder.ToTable("addresses");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.AddressLine).HasColumnName("address").IsRequired();
        builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();

        builder.HasOne(d => d.User).WithMany(p => p.Addresses)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("addresses_user_id_fkey");
    }
}