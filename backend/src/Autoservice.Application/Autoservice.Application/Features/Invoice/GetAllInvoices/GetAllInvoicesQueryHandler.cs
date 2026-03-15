using Autoservice.Application.DTOs.Common;
using Autoservice.Application.DTOs.ServiceInvoice;
using Autoservice.Application.Interfaces;
using Autoservice.Domain.Repositories;
using MediatR;


namespace Autoservice.Application.Features.Invoice.GetAllInvoices;

public sealed class GetAllInvoicesQueryHandler(
    IServiceInvoiceRepository invoices,
    IEventLogger logger
) : IRequestHandler<GetAllInvoicesQuery, Result<IEnumerable<ServiceInvoiceResponseDto>>>
{
    public async Task<Result<IEnumerable<ServiceInvoiceResponseDto>>> Handle(
        GetAllInvoicesQuery request,
        CancellationToken ct)
    {
        logger.Info("Manager requested all service invoices.");

        var all = await invoices.GetAllAsync(ct);

        var response = all.Select(i => new ServiceInvoiceResponseDto
        {
            Id = i.Id,
            MechanicName = i.MechanicName,
            IssuedAt = i.IssuedAt,
            TotalAmount = i.TotalAmount,
            VehicleLicensePlate = i.Vehicle.LicensePlate,
            VehicleBrand = i.Vehicle.Brand,
            VehicleModel = i.Vehicle.Model
        }).ToList();

        logger.Info($"Returned {response.Count} invoice(s).");

        return Result<IEnumerable<ServiceInvoiceResponseDto>>.Success(response);
    }
}
