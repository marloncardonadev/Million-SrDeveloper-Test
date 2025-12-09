using Microsoft.EntityFrameworkCore;
using Million.RealEstate.Backend.Domain.Entities;
using Million.RealEstate.Backend.Domain.Interfaces;
using Million.RealEstate.Backend.Infrastructure.Persistence;

namespace Million.RealEstate.Backend.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly RealEstateDbContext _context;

    public UserRepository(RealEstateDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }
}
