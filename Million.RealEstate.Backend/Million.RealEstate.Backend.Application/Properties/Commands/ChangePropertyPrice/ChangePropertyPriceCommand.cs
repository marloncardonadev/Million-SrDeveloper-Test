using MediatR;

namespace Million.RealEstate.Backend.Application.Properties.Commands.ChangePropertyPrice;

public record ChangePropertyPriceCommand(int PropertyId, decimal NewPrice) : IRequest;
