using MediatR;
using Million.RealEstate.Backend.Domain.Common;
using Million.RealEstate.Backend.Domain.Entities;
using Million.RealEstate.Backend.Domain.Interfaces;

namespace Million.RealEstate.Backend.Application.Properties.Commands.CreateProperty;

public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, int>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePropertyCommandHandler(
        IPropertyRepository propertyRepository,
        IOwnerRepository ownerRepository,
        IUnitOfWork unitOfWork)
    {
        _propertyRepository = propertyRepository;
        _ownerRepository = ownerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
    {
        var owner = await _ownerRepository.GetByIdAsync(request.OwnerId, cancellationToken);
        if (owner is null)
            throw new DomainException($"Owner {request.OwnerId} not found.");

        var existsCode = await _propertyRepository.ExistsCodeInternalAsync(request.CodeInternal, cancellationToken);
        if (existsCode)
            throw new DomainException("CodeInternal already exists.");

        var property = new Property(
            request.Name,
            request.Address,
            request.Price,
            request.CodeInternal,
            request.Year,
            request.OwnerId);

        await _propertyRepository.AddAsync(property, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return property.Id;
    }
}
