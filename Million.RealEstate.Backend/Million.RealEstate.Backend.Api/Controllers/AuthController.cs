using MediatR;
using Microsoft.AspNetCore.Mvc;
using Million.RealEstate.Backend.Api.Models.Auth;
using Million.RealEstate.Backend.Application.Dtos;
using Million.RealEstate.Backend.Application.Users.Commands.LoginUser;
using Million.RealEstate.Backend.Application.Users.Commands.RegisterUser;

namespace Million.RealEstate.Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Registers a new user with the provided registration details.
    /// </summary>
    /// <remarks>This method processes the registration request by sending a <see cref="RegisterUserCommand"/>
    /// to the mediator.  The result includes authentication information for the registered user.</remarks>
    /// <param name="request">The registration details, including the username, email, and password.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="AuthResultDto"/> with the authentication result  for
    /// the newly registered user.</returns>
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
    /// Authenticates a user based on the provided login credentials.
    /// </summary>
    /// <remarks>This method processes the login request by sending a <see cref="LoginUserCommand"/> to the
    /// mediator. The result includes authentication details such as tokens or user information.</remarks>
    /// <param name="request">The login request containing the user's username or email and password.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="AuthResultDto"/> with authentication details if the
    /// login is successful.</returns>
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
