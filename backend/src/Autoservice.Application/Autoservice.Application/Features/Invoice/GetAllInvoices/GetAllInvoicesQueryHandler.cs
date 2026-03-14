using Autoservice.Application.DTOs.Common;
using Autoservice.Application.DTOs.ServiceInvoice;
using Autoservice.Domain.Repositories;
using MediatR;

namespace Autoservice.Application.Features.Invoice.GetAllInvoices;

public sealed class GetAllInvoicesQueryHandler(
    IServiceInvoiceRepository invoices
) : IRequestHandler<GetAllInvoicesQuery, Result<IEnumerable<ServiceInvoiceResponseDto>>>
{
    public async Task<Result<IEnumerable<ServiceInvoiceResponseDto>>> Handle(
        GetAllInvoicesQuery request,
        CancellationToken ct)
    {
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
        });

        return Result<IEnumerable<ServiceInvoiceResponseDto>>.Success(response);
    }
}
