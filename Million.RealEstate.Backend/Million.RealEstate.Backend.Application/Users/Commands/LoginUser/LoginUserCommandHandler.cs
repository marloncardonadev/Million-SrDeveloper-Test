using MediatR;
using Million.RealEstate.Backend.Application.Abstractions;
using Million.RealEstate.Backend.Application.Abstractions.Security;
using Million.RealEstate.Backend.Application.Dtos;
using Million.RealEstate.Backend.Domain.Common;
using Million.RealEstate.Backend.Domain.Interfaces;

namespace Million.RealEstate.Backend.Application.Users.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResultDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<AuthResultDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUserNameAsync(request.UserNameOrEmail, cancellationToken)
                   ?? await _userRepository.GetByEmailAsync(request.UserNameOrEmail, cancellationToken);

        if (user is null || !user.IsActive)
            throw new DomainException("Invalid credentials.");

        var valid = _passwordHasher.VerifyPassword(user.PasswordHash, request.Password);
        if (!valid)
            throw new DomainException("Invalid credentials.");

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName, user.Email);

        return new AuthResultDto
        {
            Token = token,
            UserName = user.UserName,
            Email = user.Email,
            ExpiresAt = _dateTimeProvider.UtcNow.AddHours(1)
        };
    }
}
