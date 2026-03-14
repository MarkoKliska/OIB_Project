namespace Autoservice.Application.DTOs.ServiceInvoice;

public class ServiceInvoiceResponseDto
{
    public Guid Id { get; set; }
    public string MechanicName { get; set; } = default!;
    public DateTime IssuedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public string VehicleLicensePlate { get; set; } = default!;
    public string VehicleBrand { get; set; } = default!;
    public string VehicleModel { get; set; } = default!;
}
