namespace Autoservice.Application.DTOs.User.Login;
public class LoginResponseDto
{
    public string Token { get; set; } = default!;
    public string Role { get; set; } = default!;
    public string FullName { get; set; } = default!;
}
