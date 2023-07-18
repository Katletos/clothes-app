using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.Email).IsUnique();

        builder.Property(e => e.Id);
        builder.Property(e => e.CreatedAt).IsRequired()
            .HasColumnType("timestamp without time zone");
        builder.Property(e => e.UserType).IsRequired();
        builder.Property(e => e.Email).IsRequired();
        builder.Property(e => e.Password).IsRequired();
        builder.Property(e => e.FirstName);
        builder.Property(e => e.LastName);
        builder.Property(e => e.Phone);
    }
}