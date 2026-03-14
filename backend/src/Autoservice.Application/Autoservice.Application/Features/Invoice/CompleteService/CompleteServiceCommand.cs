using Autoservice.Application.DTOs.Common;
using Autoservice.Application.DTOs.ServiceInvoice;
using MediatR;

namespace Autoservice.Application.Features.Invoice.CompleteService;

public sealed record CompleteServiceCommand(Guid VehicleId, Guid MechanicId, string MechanicName)
    : IRequest<Result<ServiceInvoiceResponseDto>>;
