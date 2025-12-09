using Microsoft.EntityFrameworkCore;
using Million.RealEstate.Backend.Domain.Entities;
using Million.RealEstate.Backend.Domain.Interfaces;

namespace Million.RealEstate.Backend.Infrastructure.Persistence;

public class RealEstateDbContext : DbContext, IUnitOfWork
{
    public DbSet<Owner> Owners => Set<Owner>();
    public DbSet<Property> Properties => Set<Property>();
    public DbSet<PropertyImage> PropertyImages => Set<PropertyImage>();
    public DbSet<PropertyTrace> PropertyTraces => Set<PropertyTrace>();
    public DbSet<User> Users => Set<User>();

    public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options)
        : base(options)
    {
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => base.SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RealEstateDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
