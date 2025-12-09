using FluentValidation;

namespace Million.RealEstate.Backend.Application.Properties.Commands.CreateProperty;

public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
{
    public CreatePropertyCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Address)
            .NotEmpty();

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.CodeInternal)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Year)
            .InclusiveBetween(1900, DateTime.UtcNow.Year);

        RuleFor(x => x.OwnerId)
            .NotEmpty();
    }
}
