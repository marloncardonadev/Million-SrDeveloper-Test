using Million.RealEstate.Backend.Domain.Entities;

namespace Million.RealEstate.Backend.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
}
