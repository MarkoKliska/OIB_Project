using Autoservice.Application.DTOs.Vehicle;
using Autoservice.Application.Features.Vehicle.AddVehicle;
using Autoservice.Application.Features.Vehicle.GetAllVehicles;
using Autoservice.Application.Features.Vehicle.GetUnservicedVehicles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autoservice.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VehiclesController(IMediator mediator) : ControllerBase
{
    // Manager: add a new vehicle (max 10 active)
    [HttpPost]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<IActionResult> AddVehicle([FromBody] AddVehicleRequestDto request, CancellationToken ct)
    {
        var result = await mediator.Send(new AddVehicleCommand(request), ct);
        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });
        return Ok(result.Value);
    }

    // Manager: get all vehicles (serviced and waiting)
    [HttpGet]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllVehiclesQuery(), ct);
        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });
        return Ok(result.Value);
    }

    // Mechanic: get only unserviced vehicles
    [HttpGet("unserviced")]
    [Authorize(Policy = "MechanicOnly")]
    public async Task<IActionResult> GetUnserviced(CancellationToken ct)
    {
        var result = await mediator.Send(new GetUnservicedVehiclesQuery(), ct);
        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });
        return Ok(result.Value);
    }
}
