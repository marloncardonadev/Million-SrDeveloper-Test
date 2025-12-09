namespace Million.RealEstate.Backend.Application.Dtos;

public class PropertyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public decimal Price { get; set; }
    public string CodeInternal { get; set; } = default!;
    public int Year { get; set; }
    public int OwnerId { get; set; }
}
