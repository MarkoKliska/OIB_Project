namespace Autoservice.Domain.Entities;
public enum VehicleType
{
    Passenger,
    Truck,
    Motorcycle
}
public class Vehicle
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string LicensePlate { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public VehicleType Type { get; set; }
    public decimal EstimatedPrice { get; set; }
    public bool IsServiced { get; set; } = false;

    public ServiceInvoice? ServiceInvoice { get; set; }
}
