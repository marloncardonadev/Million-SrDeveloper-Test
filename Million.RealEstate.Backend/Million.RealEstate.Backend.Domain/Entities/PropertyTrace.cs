using Million.RealEstate.Backend.Domain.Common;

namespace Million.RealEstate.Backend.Domain.Entities;

public class PropertyTrace : Entity
{
    public DateTime DateSale { get; private set; }
    public string Name { get; private set; } = default!;
    public decimal Value { get; private set; }
    public decimal Tax { get; private set; }
    public int PropertyId { get; private set; }

    private PropertyTrace() { }

    public PropertyTrace(DateTime dateSale, string name, decimal value, decimal tax, int propertyId)
    {
        DateSale = dateSale;
        Name = name;
        Value = value;
        Tax = tax;
        PropertyId = propertyId;
    }
}
