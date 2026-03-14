using Autoservice.Application.Features.Invoice.CompleteService;
using Autoservice.Application.Features.Invoice.GetAllInvoices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Autoservice.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvoicesController(IMediator mediator) : ControllerBase
{
    // Manager: get all issued invoices
    [HttpGet]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllInvoicesQuery(), ct);
        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });
        return Ok(result.Value);
    }

    // Mechanic: complete service for a vehicle and issue invoice
    [HttpPost("complete/{vehicleId:guid}")]
    [Authorize(Policy = "MechanicOnly")]
    public async Task<IActionResult> CompleteService(Guid vehicleId, CancellationToken ct)
    {
        var mechanicIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var mechanicNameClaim = User.FindFirstValue("FullName");

        if (mechanicIdClaim is null || !Guid.TryParse(mechanicIdClaim, out var mechanicId))
            return Unauthorized(new { error = "Invalid token." });

        var mechanicName = mechanicNameClaim ?? User.FindFirstValue(ClaimTypes.Name) ?? "Unknown";

        var result = await mediator.Send(new CompleteServiceCommand(vehicleId, mechanicId, mechanicName), ct);
        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });
        return Ok(result.Value);
    }
}
