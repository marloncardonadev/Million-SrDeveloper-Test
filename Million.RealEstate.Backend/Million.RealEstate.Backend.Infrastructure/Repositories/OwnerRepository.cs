using Microsoft.EntityFrameworkCore;
using Million.RealEstate.Backend.Domain.Entities;
using Million.RealEstate.Backend.Domain.Interfaces;
using Million.RealEstate.Backend.Infrastructure.Persistence;

namespace Million.RealEstate.Backend.Infrastructure.Repositories;

public class OwnerRepository : IOwnerRepository
{
    private readonly RealEstateDbContext _context;

    public OwnerRepository(RealEstateDbContext context)
    {
        _context = context;
    }

    public Task<Owner?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Owners.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }
}
