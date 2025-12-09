namespace Million.RealEstate.Backend.Api.Models;

public class UpdatePropertyRequest
{
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public int Year { get; set; }
    public int OwnerId { get; set; }
}
