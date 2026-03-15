using Autoservice.Application.DTOs.Common;
using Autoservice.Application.DTOs.Vehicle;
using MediatR;

namespace Autoservice.Application.Features.Vehicle.GetAllVehicles;

public sealed record GetAllVehiclesQuery() 
    : IRequest<Result<IEnumerable<VehicleResponseDto>>>;
