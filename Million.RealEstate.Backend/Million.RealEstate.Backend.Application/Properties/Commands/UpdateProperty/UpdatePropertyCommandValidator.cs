using FluentValidation;

namespace Million.RealEstate.Backend.Application.Properties.Commands.UpdateProperty;

public class UpdatePropertyCommandValidator : AbstractValidator<UpdatePropertyCommand>
{
    public UpdatePropertyCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Year).InclusiveBetween(1900, DateTime.UtcNow.Year);
        RuleFor(x => x.OwnerId).NotEmpty();
    }
}
