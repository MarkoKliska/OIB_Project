using Autoservice.Application.DTOs.Common;
using Autoservice.Application.DTOs.Vehicle;
using Autoservice.Application.Interfaces;
using Autoservice.Domain.Repositories;
using MediatR;

namespace Autoservice.Application.Features.Vehicle.GetAllVehicles;

public sealed class GetAllVehiclesQueryHandler(
    IVehicleRepository vehicles,
    IEventLogger logger
) : IRequestHandler<GetAllVehiclesQuery, Result<IEnumerable<VehicleResponseDto>>>
{
    public async Task<Result<IEnumerable<VehicleResponseDto>>> Handle(
        GetAllVehiclesQuery request,
        CancellationToken ct)
    {
        logger.Info("Manager requested all vehicles.");

        var all = await vehicles.GetAllAsync(ct);

        var response = all.Select(v => new VehicleResponseDto
        {
            Id = v.Id,
            LicensePlate = v.LicensePlate,
            Brand = v.Brand,
            Model = v.Model,
            Type = v.Type.ToString(),
            EstimatedPrice = v.EstimatedPrice,
            IsServiced = v.IsServiced
        }).ToList();

        logger.Info($"Returned {response.Count} vehicle(s) total.");

        return Result<IEnumerable<VehicleResponseDto>>.Success(response);
    }
}
