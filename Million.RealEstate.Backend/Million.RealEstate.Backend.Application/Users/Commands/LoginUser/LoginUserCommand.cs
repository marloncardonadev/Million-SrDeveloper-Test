using MediatR;
using Million.RealEstate.Backend.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.RealEstate.Backend.Application.Users.Commands.LoginUser;

public record LoginUserCommand(
    string UserNameOrEmail,
    string Password
) : IRequest<AuthResultDto>;
