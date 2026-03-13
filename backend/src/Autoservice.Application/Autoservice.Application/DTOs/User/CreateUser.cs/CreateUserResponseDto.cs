namespace Autoservice.Application.DTOs.User.CreateUser;

public class CreateUserResponseDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Role { get; set; } = default!;
    public string Token { get; set; } = default!;
}
