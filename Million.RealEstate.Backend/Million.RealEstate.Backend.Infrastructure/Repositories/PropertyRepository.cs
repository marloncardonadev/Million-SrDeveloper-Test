using Microsoft.EntityFrameworkCore;
using Million.RealEstate.Backend.Domain.Entities;
using Million.RealEstate.Backend.Domain.Interfaces;
using Million.RealEstate.Backend.Infrastructure.Persistence;

namespace Million.RealEstate.Backend.Infrastructure.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly RealEstateDbContext _context;

    public PropertyRepository(RealEstateDbContext context)
    {
        _context = context;
    }

    public async Task<Property?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Properties
            .Include(p => p.Images)
            .Include(p => p.Traces)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task AddAsync(Property property, CancellationToken cancellationToken = default)
    {
        await _context.Properties.AddAsync(property, cancellationToken);
    }

    public void Update(Property property)
    {
        _context.Properties.Update(property);
    }

    public async Task<bool> ExistsCodeInternalAsync(string codeInternal, CancellationToken cancellationToken = default)
    {
        return await _context.Properties.AnyAsync(p => p.CodeInternal == codeInternal, cancellationToken);
    }

    public IQueryable<Property> Query()
    {
        return _context.Properties.AsQueryable();
    }
}
