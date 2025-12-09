using Million.RealEstate.Backend.Domain.Entities;

namespace Million.RealEstate.Backend.Domain.Interfaces;

public interface IPropertyRepository
{
    Task<Property?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Property property, CancellationToken cancellationToken = default);
    void Update(Property property);
    Task<bool> ExistsCodeInternalAsync(string codeInternal, CancellationToken cancellationToken = default);
    IQueryable<Property> Query(); // para filtros
}
