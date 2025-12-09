using Million.RealEstate.Backend.Domain.Common;

namespace Million.RealEstate.Backend.Domain.Entities;

public class Property : AggregateRoot
{
    public string Name { get; private set; } = default!;
    public string Address { get; private set; } = default!;
    public decimal Price { get; private set; }
    public string CodeInternal { get; private set; } = default!;
    public int Year { get; private set; }
    public int OwnerId { get; private set; }

    private readonly List<PropertyImage> _images = new();
    public IReadOnlyCollection<PropertyImage> Images => _images.AsReadOnly();

    private readonly List<PropertyTrace> _traces = new();
    public IReadOnlyCollection<PropertyTrace> Traces => _traces.AsReadOnly();

    private Property() { }

    public Property(
        string name,
        string address,
        decimal price,
        string codeInternal,
        int year,
        int ownerId)
    {
        UpdateBasicInfo(name, address, year);
        SetPrice(price);
        CodeInternal = codeInternal;
        OwnerId = ownerId;
    }

    public void UpdateBasicInfo(string name, string address, int year)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name is required.");

        if (string.IsNullOrWhiteSpace(address))
            throw new DomainException("Address is required.");

        if (year < 1900 || year > DateTime.UtcNow.Year)
            throw new DomainException("Year is invalid.");

        Name = name;
        Address = address;
        Year = year;
    }

    public void SetPrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new DomainException("Price must be greater than zero.");

        Price = newPrice;
    }

    public PropertyImage AddImage(string file)
    {
        if (string.IsNullOrWhiteSpace(file))
            throw new DomainException("File is required.");

        var image = new PropertyImage(Id, file);
        _images.Add(image);
        return image;
    }

    public PropertyTrace AddTrace(DateTime dateSale, string name, decimal value, decimal tax)
    {
        var trace = new PropertyTrace(dateSale, name, value, tax, Id);
        _traces.Add(trace);
        return trace;
    }
}
