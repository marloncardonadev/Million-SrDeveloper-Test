using MediatR;

namespace Million.RealEstate.Backend.Application.Properties.Commands.AddPropertyImage;

public record AddPropertyImageCommand(int PropertyId, string File) : IRequest<int>;
