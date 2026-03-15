using Autoservice.Application.Authentication;
using Autoservice.Application.DTOs.Common;
using Autoservice.Application.DTOs.User.CreateUser;
using Autoservice.Application.Interfaces;
using Autoservice.Application.Utils;
using Autoservice.Domain.Entities;
using Autoservice.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Autoservice.Application.Features.User.CreateUser;

public sealed class CreateUserCommandHandler(
    IUserRepository users,
    IUnitOfWork uow,
    IJwtTokenService jwtService,
    IEventLogger logger
) : IRequestHandler<CreateUserCommand, Result<CreateUserResponseDto>>
{
    public async Task<Result<CreateUserResponseDto>> Handle(
        CreateUserCommand command,
        CancellationToken ct)
    {
        var req = command.Request;

        logger.Info($"Attempting to register new user with username '{req.Username}'.");

        var exists = await users.GetByUsernameAsync(req.Username, ct);
        if (exists is not null)
        {
            logger.Warning($"Registration failed - username '{req.Username}' already exists.");
            return Result<CreateUserResponseDto>.Failure("Username already exists.");
        }

        var roleEnum = Enum.TryParse<UserRole>(req.Role, ignoreCase: true, out var parsed)
            ? parsed
            : UserRole.Mechanic;

        var user = new Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            FirstName = req.FirstName,
            LastName = req.LastName,
            Username = req.Username,
            PasswordHash = PasswordHasher.HashPassword(req.Password),
            Role = roleEnum
        };

        await users.AddAsync(user, ct);
        await uow.SaveChangesAsync(ct);

        var token = jwtService.GenerateToken(user.Id, user.Username, user.Role, user.FullName);

        logger.Info($"User '{req.Username}' successfully registered with role '{roleEnum}'.");

        return Result<CreateUserResponseDto>.Success(new CreateUserResponseDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Role = user.Role.ToString(),
            Token = token
        });
    }
}
