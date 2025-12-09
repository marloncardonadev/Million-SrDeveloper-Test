using MediatR;

namespace Million.RealEstate.Backend.Application.Properties.Commands.CreateProperty;

public record CreatePropertyCommand(
    string Name,
    string Address,
    decimal Price,
    string CodeInternal,
    int Year,
    int OwnerId
) : IRequest<int>;
