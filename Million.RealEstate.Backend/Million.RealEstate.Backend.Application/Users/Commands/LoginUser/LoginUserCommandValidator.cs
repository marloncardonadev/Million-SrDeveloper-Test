using FluentValidation;

namespace Million.RealEstate.Backend.Application.Users.Commands.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.UserNameOrEmail).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
