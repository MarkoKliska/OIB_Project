using Autoservice.Application.DTOs.Common;
using Autoservice.Application.DTOs.Vehicle;
using Autoservice.Application.Interfaces;
using Autoservice.Domain.Repositories;
using MediatR;

namespace Autoservice.Application.Features.Vehicle.GetUnservicedVehicles;

public sealed class GetUnservicedVehiclesQueryHandler(
    IVehicleRepository vehicles,
    IEventLogger logger
) : IRequestHandler<GetUnservicedVehiclesQuery, Result<IEnumerable<VehicleResponseDto>>>
{
    public async Task<Result<IEnumerable<VehicleResponseDto>>> Handle(
        GetUnservicedVehiclesQuery request,
        CancellationToken ct)
    {
        logger.Info("Mechanic requested list of unserviced vehicles.");

        var unserviced = await vehicles.GetUnservicedAsync(ct);

        var response = unserviced.Select(v => new VehicleResponseDto
        {
            Id = v.Id,
            LicensePlate = v.LicensePlate,
            Brand = v.Brand,
            Model = v.Model,
            Type = v.Type.ToString(),
            EstimatedPrice = v.EstimatedPrice,
            IsServiced = v.IsServiced
        }).ToList();

        logger.Info($"Returned {response.Count} unserviced vehicle(s).");

        return Result<IEnumerable<VehicleResponseDto>>.Success(response);
    }
}
