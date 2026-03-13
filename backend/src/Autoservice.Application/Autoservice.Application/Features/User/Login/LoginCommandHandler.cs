using Autoservice.Application.Authentication;
using Autoservice.Application.DTOs.Common;
using Autoservice.Application.DTOs.User.Login;
using Autoservice.Application.Utils;
using Autoservice.Domain.Repositories;
using MediatR;

namespace Autoservice.Application.Features.User.Login;

public sealed class LoginCommandHandler(
    IUserRepository users,
    IJwtTokenService jwtService
) : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
{
    public async Task<Result<LoginResponseDto>> Handle(
        LoginCommand command,
        CancellationToken ct)
    {
        var req = command.Request;

        var user = await users.GetByUsernameAsync(req.Username, ct);

        if (user is null || !PasswordHasher.VerifyPassword(req.Password, user.PasswordHash))
            return Result<LoginResponseDto>.Failure("Invalid username or password.");

        var token = jwtService.GenerateToken(user.Id, user.Username, user.Role);

        return Result<LoginResponseDto>.Success(new LoginResponseDto
        {
            Token = token,
            Role = user.Role.ToString(),
            FullName = user.FullName
        });
    }
}
