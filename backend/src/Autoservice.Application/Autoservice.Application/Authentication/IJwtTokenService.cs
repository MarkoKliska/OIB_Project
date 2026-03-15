using Autoservice.Domain.Entities;

namespace Autoservice.Application.Authentication;

public interface IJwtTokenService
{
    string GenerateToken(Guid userId, string username, UserRole role, string fullName = "");
}