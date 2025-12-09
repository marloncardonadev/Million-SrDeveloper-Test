using MediatR;
using Million.RealEstate.Backend.Domain.Common;
using Million.RealEstate.Backend.Domain.Interfaces;

namespace Million.RealEstate.Backend.Application.Properties.Commands.AddPropertyImage;

public class AddPropertyImageCommandHandler : IRequestHandler<AddPropertyImageCommand, int>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddPropertyImageCommandHandler(
        IPropertyRepository propertyRepository,
        IUnitOfWork unitOfWork)
    {
        _propertyRepository = propertyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(AddPropertyImageCommand request, CancellationToken cancellationToken)
    {
        var property = await _propertyRepository.GetByIdAsync(request.PropertyId, cancellationToken);

        if (property is null)
            throw new DomainException($"Property {request.PropertyId} not found.");

        var image = property.AddImage(request.File);

        _propertyRepository.Update(property);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return image.Id;
    }
}
