using Autoservice.Application.DTOs.User.CreateUser;
using Autoservice.Application.DTOs.User.Login;
using Autoservice.Application.Features.User.CreateUser;
using Autoservice.Application.Features.User.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Autoservice.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserRequestDto request, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateUserCommand(request), ct);
        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });
        return Ok(result.Value);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken ct)
    {
        var result = await mediator.Send(new LoginCommand(request), ct);
        if (!result.IsSuccess)
            return Unauthorized(new { error = result.Error });
        return Ok(result.Value);
    }
}
