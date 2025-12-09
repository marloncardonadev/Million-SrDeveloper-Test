using MediatR;
using Million.RealEstate.Backend.Application.Dtos;

namespace Million.RealEstate.Backend.Application.Properties.Queries.GetPropertiesWithFilters;

public class GetPropertiesWithFiltersQuery : IRequest<List<PropertyDto>>
{
    public string? Name { get; init; }
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public int? Year { get; init; }
    public int? OwnerId { get; init; }
    public int? Page { get; init; }
    public int? PageSize { get; init; }
    public string? SortBy { get; init; }
    public bool Desc { get; init; }
}
