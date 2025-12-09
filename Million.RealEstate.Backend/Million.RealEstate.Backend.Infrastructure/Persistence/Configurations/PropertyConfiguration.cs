using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Million.RealEstate.Backend.Domain.Entities;

namespace Million.RealEstate.Backend.Infrastructure.Persistence.Configurations;

public class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.ToTable("Property");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Address)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(p => p.CodeInternal)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(p => p.CodeInternal)
            .IsUnique();

        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        builder.HasOne<Owner>()
            .WithMany()
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Images)
            .WithOne()
            .HasForeignKey(i => i.PropertyId);

        builder.HasMany(p => p.Traces)
            .WithOne()
            .HasForeignKey(t => t.PropertyId);
    }
}
