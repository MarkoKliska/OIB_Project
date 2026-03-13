using Autoservice.Application.DTOs.Common;
using Autoservice.Application.DTOs.User.CreateUser;
using MediatR;

namespace Autoservice.Application.Features.User.CreateUser;

public sealed record CreateUserCommand(CreateUserRequestDto Request)
    : IRequest<Result<CreateUserResponseDto>>;
