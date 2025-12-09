using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Million.RealEstate.Backend.Application.Dtos;
using Million.RealEstate.Backend.Domain.Interfaces;

namespace Million.RealEstate.Backend.Application.Properties.Queries.GetPropertiesWithFilters;

public class GetPropertiesWithFiltersQueryHandler : IRequestHandler<GetPropertiesWithFiltersQuery, List<PropertyDto>>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public GetPropertiesWithFiltersQueryHandler(
        IPropertyRepository propertyRepository,
        IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
    }

    public async Task<List<PropertyDto>> Handle(GetPropertiesWithFiltersQuery request, CancellationToken cancellationToken)
    {
        var query = _propertyRepository.Query();

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(p => p.Name.Contains(request.Name));

        if (request.MinPrice.HasValue)
            query = query.Where(p => p.Price >= request.MinPrice.Value);

        if (request.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= request.MaxPrice.Value);

        if (request.Year.HasValue)
            query = query.Where(p => p.Year == request.Year.Value);

        if (request.OwnerId.HasValue)
            query = query.Where(p => p.OwnerId == request.OwnerId.Value);

        if (!string.IsNullOrEmpty(request.SortBy))
        {
            query = request.SortBy.ToLower() switch
            {
                "price" => request.Desc ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price),
                "year" => request.Desc ? query.OrderByDescending(x => x.Year) : query.OrderBy(x => x.Year),
                _ => query.OrderBy(x => x.Name)
            };
        }

        if (request.Page.HasValue && request.PageSize.HasValue)
        {
            int skip = (request.Page.Value - 1) * request.PageSize.Value;
            query = query.Skip(skip).Take(request.PageSize.Value);
        }

        return await query
            .ProjectTo<PropertyDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
