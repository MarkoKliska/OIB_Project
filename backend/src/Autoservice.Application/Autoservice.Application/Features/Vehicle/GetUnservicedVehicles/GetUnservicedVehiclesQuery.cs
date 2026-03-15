using Autoservice.Application.DTOs.Common;
using Autoservice.Application.DTOs.Vehicle;
using MediatR;

namespace Autoservice.Application.Features.Vehicle.GetUnservicedVehicles;

public sealed record GetUnservicedVehiclesQuery() 
    : IRequest<Result<IEnumerable<VehicleResponseDto>>>;
