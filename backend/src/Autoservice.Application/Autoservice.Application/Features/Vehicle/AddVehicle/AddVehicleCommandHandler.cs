using Autoservice.Application.DTOs.Common;
using Autoservice.Application.DTOs.Vehicle;
using Autoservice.Application.Interfaces;
using Autoservice.Domain.Entities;
using Autoservice.Domain.Repositories;
using MediatR;

namespace Autoservice.Application.Features.Vehicle.AddVehicle;

public sealed class AddVehicleCommandHandler(
    IVehicleRepository vehicles,
    IUnitOfWork uow
) : IRequestHandler<AddVehicleCommand, Result<VehicleResponseDto>>
{
    private const int MaxVehiclesOnService = 10;

    public async Task<Result<VehicleResponseDto>> Handle(
        AddVehicleCommand command,
        CancellationToken ct)
    {
        var req = command.Request;

        var activeCount = await vehicles.GetActiveCountAsync(ct);
        if (activeCount >= MaxVehiclesOnService)
            return Result<VehicleResponseDto>.Failure(
                $"Cannot add vehicle. Maximum capacity of {MaxVehiclesOnService} vehicles on service has been reached.");

        var activeWithSamePlate = await vehicles.GetActiveByLicensePlateAsync(req.LicensePlate, ct);
        if (activeWithSamePlate is not null)
            return Result<VehicleResponseDto>.Failure(
                $"Vehicle with license plate '{req.LicensePlate}' is already on service.");

        if (!Enum.TryParse<VehicleType>(req.Type, ignoreCase: true, out var vehicleType))
            return Result<VehicleResponseDto>.Failure(
                $"Invalid vehicle type '{req.Type}'. Valid values: Passenger, Truck, Motorcycle.");

        var vehicle = new Domain.Entities.Vehicle
        {
            Id = Guid.NewGuid(),
            LicensePlate = req.LicensePlate,
            Brand = req.Brand,
            Model = req.Model,
            Type = vehicleType,
            EstimatedPrice = req.EstimatedPrice,
            IsServiced = false
        };

        await vehicles.AddAsync(vehicle, ct);
        await uow.SaveChangesAsync(ct);

        return Result<VehicleResponseDto>.Success(new VehicleResponseDto
        {
            Id = vehicle.Id,
            LicensePlate = vehicle.LicensePlate,
            Brand = vehicle.Brand,
            Model = vehicle.Model,
            Type = vehicle.Type.ToString(),
            EstimatedPrice = vehicle.EstimatedPrice,
            IsServiced = vehicle.IsServiced
        });
    }
}
