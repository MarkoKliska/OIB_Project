namespace Autoservice.Application.DTOs.User.CreateUser;

public class CreateUserRequestDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Role { get; set; } = "Mechanic";
}
