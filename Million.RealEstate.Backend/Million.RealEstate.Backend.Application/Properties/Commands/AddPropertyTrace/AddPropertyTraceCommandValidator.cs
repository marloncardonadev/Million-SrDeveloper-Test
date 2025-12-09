using FluentValidation;

namespace Million.RealEstate.Backend.Application.Properties.Commands.AddPropertyTrace;

public class AddPropertyTraceCommandValidator : AbstractValidator<AddPropertyTraceCommand>
{
    public AddPropertyTraceCommandValidator()
    {
        RuleFor(x => x.PropertyId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Value).GreaterThan(0);
        RuleFor(x => x.Tax).GreaterThanOrEqualTo(0);
    }
}
