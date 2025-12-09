using MediatR;
using Million.RealEstate.Backend.Domain.Common;
using Million.RealEstate.Backend.Domain.Interfaces;

namespace Million.RealEstate.Backend.Application.Properties.Commands.ChangePropertyPrice;

public class ChangePropertyPriceCommandHandler : IRequestHandler<ChangePropertyPriceCommand>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePropertyPriceCommandHandler(
        IPropertyRepository propertyRepository,
        IUnitOfWork unitOfWork)
    {
        _propertyRepository = propertyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ChangePropertyPriceCommand request, CancellationToken cancellationToken)
    {
        var property = await _propertyRepository.GetByIdAsync(request.PropertyId, cancellationToken);

        if (property is null)
            throw new DomainException($"Property {request.PropertyId} not found.");

        property.SetPrice(request.NewPrice);

        _propertyRepository.Update(property);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
