using MediatR;

namespace Million.RealEstate.Backend.Application.Properties.Commands.UpdateProperty;

public record UpdatePropertyCommand(
    int Id,
    string Name,
    string Address,
    int Year,
    int OwnerId
) : IRequest<Unit>;