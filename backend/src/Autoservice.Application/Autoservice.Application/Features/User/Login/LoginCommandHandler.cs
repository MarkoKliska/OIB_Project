using Autoservice.Application.Authentication;
using Autoservice.Application.DTOs.Common;
using Autoservice.Application.DTOs.User.Login;
using Autoservice.Application.Interfaces;
using Autoservice.Application.Utils;
using Autoservice.Domain.Repositories;
using MediatR;

namespace Autoservice.Application.Features.User.Login;

public sealed class LoginCommandHandler(
    IUserRepository users,
    IJwtTokenService jwtService,
    IEventLogger logger
) : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
{
    public async Task<Result<LoginResponseDto>> Handle(
        LoginCommand command,
        CancellationToken ct)
    {
        var req = command.Request;

        logger.Info($"Attempting to login with username '{req.Username}'.");

        var user = await users.GetByUsernameAsync(req.Username, ct);

        if (user is null || !PasswordHasher.VerifyPassword(req.Password, user.PasswordHash))
        {
            logger.Warning($"Login failed - username or password are incorrect.");
            return Result<LoginResponseDto>.Failure("Invalid username or password.");
        }  

        var token = jwtService.GenerateToken(user.Id, user.Username, user.Role);

        return Result<LoginResponseDto>.Success(new LoginResponseDto
        {
            Token = token,
            Role = user.Role.ToString(),
            FullName = user.FullName
        });
    }
}
