using AutoMapper;
using Million.RealEstate.Backend.Application.Dtos;
using Million.RealEstate.Backend.Domain.Entities;

namespace Million.RealEstate.Backend.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Property, PropertyDto>();
    }
}
