using Million.RealEstate.Backend.Domain.Common;

namespace Million.RealEstate.Backend.Domain.Entities;

public class PropertyImage : Entity
{
    public int PropertyId { get; private set; }
    public string File { get; private set; } = default!;
    public bool Enabled { get; private set; }

    private PropertyImage() { }

    public PropertyImage(int propertyId, string file, bool enabled = true)
    {
        PropertyId = propertyId;
        File = file;
        Enabled = enabled;
    }

    public void Disable() => Enabled = false;
}
