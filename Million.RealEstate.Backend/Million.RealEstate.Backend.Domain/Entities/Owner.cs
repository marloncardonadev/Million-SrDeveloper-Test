using Million.RealEstate.Backend.Domain.Common;

namespace Million.RealEstate.Backend.Domain.Entities;

public class Owner : AggregateRoot
{
    public string Name { get; private set; } = default!;
    public string Address { get; private set; } = default!;
    public string? Photo { get; private set; }
    public DateTime Birthday { get; private set; }

    private Owner() { }

    public Owner(string name, string address, DateTime birthday, string? photo = null)
    {
        Name = name;
        Address = address;
        Birthday = birthday;
        Photo = photo;
    }

    public void Update(string name, string address, DateTime birthday, string? photo)
    {
        Name = name;
        Address = address;
        Birthday = birthday;
        Photo = photo;
    }
}
