namespace Autoservice.Domain.Entities;
public enum UserRole
{
    Manager,
    Mechanic
}
public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public UserRole Role { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public ICollection<ServiceInvoice> IssuedInvoices { get; set; } = new List<ServiceInvoice>();
}
