using Million.RealEstate.Backend.Domain.Common;

namespace Million.RealEstate.Backend.Domain.Entities;

public class User : AggregateRoot
{
    public string UserName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;

    private User() { }

    public User(string userName, string email, string passwordHash)
    {
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
    }

    public void Deactivate() => IsActive = false;
}
