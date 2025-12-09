using MediatR;
using Million.RealEstate.Backend.Domain.Common;
using Million.RealEstate.Backend.Domain.Interfaces;

namespace Million.RealEstate.Backend.Application.Properties.Commands.AddPropertyTrace;

public class AddPropertyTraceCommandHandler : IRequestHandler<AddPropertyTraceCommand, int>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddPropertyTraceCommandHandler(
        IPropertyRepository propertyRepository,
        IUnitOfWork unitOfWork)
    {
        _propertyRepository = propertyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(AddPropertyTraceCommand request, CancellationToken cancellationToken)
    {
        var property = await _propertyRepository.GetByIdAsync(request.PropertyId, cancellationToken);

        if (property is null)
            throw new DomainException($"Property {request.PropertyId} not found.");

        var trace = property.AddTrace(request.DateSale, request.Name, request.Value, request.Tax);

        _propertyRepository.Update(property);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return trace.Id;
    }
}
