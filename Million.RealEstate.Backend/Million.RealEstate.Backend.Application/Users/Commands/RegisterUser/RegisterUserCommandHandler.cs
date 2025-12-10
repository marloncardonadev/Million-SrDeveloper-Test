using MediatR;
using Million.RealEstate.Backend.Application.Abstractions;
using Million.RealEstate.Backend.Application.Abstractions.Security;
using Million.RealEstate.Backend.Application.Dtos;
using Million.RealEstate.Backend.Domain.Common;
using Million.RealEstate.Backend.Domain.Entities;
using Million.RealEstate.Backend.Domain.Interfaces;

namespace Million.RealEstate.Backend.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResultDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<AuthResultDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingByUser = await _userRepository.GetByUserNameAsync(request.UserName, cancellationToken);
        if (existingByUser is not null)
            throw new DomainException("Username is already taken.");

        var existingByEmail = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingByEmail is not null)
            throw new DomainException("Email is already registered.");

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var user = new User(request.UserName, request.Email, passwordHash);
        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

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
