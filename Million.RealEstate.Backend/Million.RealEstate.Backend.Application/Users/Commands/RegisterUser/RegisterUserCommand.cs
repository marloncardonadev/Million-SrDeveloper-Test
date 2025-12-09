using MediatR;
using Million.RealEstate.Backend.Application.Dtos;

namespace Million.RealEstate.Backend.Application.Users.Commands.RegisterUser;

public record RegisterUserCommand(
    string UserName,
    string Email,
    string Password
) : IRequest<AuthResultDto>;
