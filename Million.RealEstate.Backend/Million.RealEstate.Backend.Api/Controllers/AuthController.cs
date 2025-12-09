using MediatR;
using Microsoft.AspNetCore.Mvc;
using Million.RealEstate.Backend.Api.Models.Auth;
using Million.RealEstate.Backend.Application.Dtos;
using Million.RealEstate.Backend.Application.Users.Commands.LoginUser;
using Million.RealEstate.Backend.Application.Users.Commands.RegisterUser;

namespace Million.RealEstate.Backend.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Registra un nuevo usuario y devuelve un JWT.
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResultDto>> Register([FromBody] RegisterRequest request)
    {
        var command = new RegisterUserCommand(
            request.UserName,
            request.Email,
            request.Password);

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Inicia sesión y devuelve un JWT.
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResultDto>> Login([FromBody] LoginRequest request)
    {
        var command = new LoginUserCommand(
            request.UserNameOrEmail,
            request.Password);

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
