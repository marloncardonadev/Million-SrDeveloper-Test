using MediatR;

namespace Million.RealEstate.Backend.Application.Properties.Commands.AddPropertyTrace;

public record AddPropertyTraceCommand(
    int PropertyId,
    DateTime DateSale,
    string Name,
    decimal Value,
    decimal Tax
) : IRequest<int>;
