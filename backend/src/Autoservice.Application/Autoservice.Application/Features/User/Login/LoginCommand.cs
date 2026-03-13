using Autoservice.Application.DTOs.Common;
using Autoservice.Application.DTOs.User.Login;
using MediatR;

namespace Autoservice.Application.Features.User.Login;

public sealed record LoginCommand(LoginRequestDto Request)
    : IRequest<Result<LoginResponseDto>>;
