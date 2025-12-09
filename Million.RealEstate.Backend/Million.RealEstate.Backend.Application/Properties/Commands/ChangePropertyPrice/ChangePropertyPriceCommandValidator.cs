using FluentValidation;

namespace Million.RealEstate.Backend.Application.Properties.Commands.ChangePropertyPrice;

public class ChangePropertyPriceCommandValidator : AbstractValidator<ChangePropertyPriceCommand>
{
    public ChangePropertyPriceCommandValidator()
    {
        RuleFor(x => x.PropertyId).NotEmpty();
        RuleFor(x => x.NewPrice).GreaterThan(0);
    }
}
