namespace Autoservice.Domain.Entities;

public class ServiceInvoice
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    public Guid MechanicId { get; set; }
    public User Mechanic { get; set; } = null!;

    public string MechanicName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
}
