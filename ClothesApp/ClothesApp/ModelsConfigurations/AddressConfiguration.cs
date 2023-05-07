using ClothesApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothesApp.ModelsConfigurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(e => e.Id).HasName("addresses_pkey");

        builder.ToTable("addresses");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Address1).HasColumnName("address");
        builder.Property(e => e.UserId).HasColumnName("user_id");

        builder.HasOne(d => d.User).WithMany(p => p.Addresses)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("addresses_user_id_fkey");
    }
}