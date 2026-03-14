namespace Autoservice.Application.DTOs.Vehicle;

public class VehicleResponseDto
{
    public Guid Id { get; set; }
    public string LicensePlate { get; set; } = default!;
    public string Brand { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string Type { get; set; } = default!;
    public decimal EstimatedPrice { get; set; }
    public bool IsServiced { get; set; }
}
