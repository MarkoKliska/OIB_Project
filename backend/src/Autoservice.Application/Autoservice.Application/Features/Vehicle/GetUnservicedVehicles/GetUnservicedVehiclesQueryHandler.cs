using Autoservice.Application.DTOs.Common;
using Autoservice.Application.DTOs.Vehicle;
using Autoservice.Domain.Repositories;
using MediatR;

namespace Autoservice.Application.Features.Vehicle.GetUnservicedVehicles;

public sealed class GetUnservicedVehiclesQueryHandler(
    IVehicleRepository vehicles
) : IRequestHandler<GetUnservicedVehiclesQuery, Result<IEnumerable<VehicleResponseDto>>>
{
    public async Task<Result<IEnumerable<VehicleResponseDto>>> Handle(
        GetUnservicedVehiclesQuery request,
        CancellationToken ct)
    {
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
        });

        return Result<IEnumerable<VehicleResponseDto>>.Success(response);
    }
}
