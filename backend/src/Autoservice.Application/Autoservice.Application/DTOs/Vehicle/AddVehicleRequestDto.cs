namespace Autoservice.Application.DTOs.Vehicle;

public class AddVehicleRequestDto
{
    public string LicensePlate { get; set; } = default!;
    public string Brand { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string Type { get; set; } = default!;
    public decimal EstimatedPrice { get; set; }
}
