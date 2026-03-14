using Autoservice.Application.DTOs.Common;
using Autoservice.Application.DTOs.Vehicle;
using Autoservice.Domain.Repositories;
using MediatR;

namespace Autoservice.Application.Features.Vehicle.GetAllVehicles;

public sealed class GetAllVehiclesQueryHandler(
    IVehicleRepository vehicles
) : IRequestHandler<GetAllVehiclesQuery, Result<IEnumerable<VehicleResponseDto>>>
{
    public async Task<Result<IEnumerable<VehicleResponseDto>>> Handle(
        GetAllVehiclesQuery request,
        CancellationToken ct)
    {
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
        });

        return Result<IEnumerable<VehicleResponseDto>>.Success(response);
    }
}
