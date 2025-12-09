using Million.RealEstate.Backend.Domain.Entities;

namespace Million.RealEstate.Backend.Domain.Interfaces;

public interface IOwnerRepository
{
    Task<Owner?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
