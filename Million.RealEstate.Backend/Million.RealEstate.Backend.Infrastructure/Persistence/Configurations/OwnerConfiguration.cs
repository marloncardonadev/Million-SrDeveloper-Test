using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Million.RealEstate.Backend.Domain.Entities;

namespace Million.RealEstate.Backend.Infrastructure.Persistence.Configurations;

public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.ToTable("Owner");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(o => o.Address)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(o => o.Photo)
            .HasMaxLength(500);
    }
}
