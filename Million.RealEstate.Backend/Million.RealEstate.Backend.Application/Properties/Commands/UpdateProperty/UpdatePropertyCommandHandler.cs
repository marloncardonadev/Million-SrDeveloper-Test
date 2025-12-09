using MediatR;
using Million.RealEstate.Backend.Domain.Common;
using Million.RealEstate.Backend.Domain.Interfaces;

namespace Million.RealEstate.Backend.Application.Properties.Commands.UpdateProperty;

public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, Unit>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePropertyCommandHandler(
        IPropertyRepository propertyRepository,
        IOwnerRepository ownerRepository,
        IUnitOfWork unitOfWork)
    {
        _propertyRepository = propertyRepository;
        _ownerRepository = ownerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
    {
        var owner = await _ownerRepository.GetByIdAsync(request.OwnerId, cancellationToken);
        if (owner is null)
            throw new DomainException($"Owner {request.OwnerId} not found.");

        var property = await _propertyRepository.GetByIdAsync(request.Id, cancellationToken);
        if (property is null)
            throw new DomainException($"Property {request.Id} not found.");

        property.UpdateBasicInfo(request.Name, request.Address, request.Year);

        _propertyRepository.Update(property);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
