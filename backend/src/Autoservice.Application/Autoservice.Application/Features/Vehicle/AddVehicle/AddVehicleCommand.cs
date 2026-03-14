using Autoservice.Application.DTOs.Common;
using Autoservice.Application.DTOs.Vehicle;
using MediatR;

namespace Autoservice.Application.Features.Vehicle.AddVehicle;

public sealed record AddVehicleCommand(AddVehicleRequestDto Request)
    : IRequest<Result<VehicleResponseDto>>;
