namespace Million.RealEstate.Backend.Api.Models;

public class CreatePropertyRequest
{
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public decimal Price { get; set; }
    public string CodeInternal { get; set; } = default!;
    public int Year { get; set; }
    public int OwnerId { get; set; }
}
