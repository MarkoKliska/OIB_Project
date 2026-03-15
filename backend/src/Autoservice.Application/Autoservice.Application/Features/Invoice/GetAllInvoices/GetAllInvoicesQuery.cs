using Autoservice.Application.DTOs.Common;
using Autoservice.Application.DTOs.ServiceInvoice;
using MediatR;

namespace Autoservice.Application.Features.Invoice.GetAllInvoices;

public sealed record GetAllInvoicesQuery() 
    : IRequest<Result<IEnumerable<ServiceInvoiceResponseDto>>>;
