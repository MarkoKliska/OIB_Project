using Autoservice.Application.DTOs.Common;
using Autoservice.Application.DTOs.ServiceInvoice;
using Autoservice.Application.Interfaces;
using Autoservice.Domain.Repositories;
using MediatR;

namespace Autoservice.Application.Features.Invoice.CompleteService;

public sealed class CompleteServiceCommandHandler(
    IVehicleRepository vehicles,
    IServiceInvoiceRepository invoices,
    IUnitOfWork uow,
    IBillingService billingService
) : IRequestHandler<CompleteServiceCommand, Result<ServiceInvoiceResponseDto>>
{
    public async Task<Result<ServiceInvoiceResponseDto>> Handle(
        CompleteServiceCommand command,
        CancellationToken ct)
    {
        var vehicle = await vehicles.GetByIdAsync(command.VehicleId, ct);

        if (vehicle is null)
            return Result<ServiceInvoiceResponseDto>.Failure("Vehicle not found.");

        if (vehicle.IsServiced)
            return Result<ServiceInvoiceResponseDto>.Failure(
                "Vehicle has already been serviced.");

        var finalAmount = billingService.CalculateFinalAmount(vehicle.EstimatedPrice);

        vehicle.IsServiced = true;
        await vehicles.UpdateAsync(vehicle, ct);

        var invoice = new Domain.Entities.ServiceInvoice
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            MechanicId = command.MechanicId,
            MechanicName = command.MechanicName,
            TotalAmount = finalAmount,
            IssuedAt = DateTime.UtcNow
        };

        await invoices.AddAsync(invoice, ct);
        await uow.SaveChangesAsync(ct);

        return Result<ServiceInvoiceResponseDto>.Success(new ServiceInvoiceResponseDto
        {
            Id = invoice.Id,
            MechanicName = invoice.MechanicName,
            IssuedAt = invoice.IssuedAt,
            TotalAmount = invoice.TotalAmount,
            VehicleLicensePlate = vehicle.LicensePlate,
            VehicleBrand = vehicle.Brand,
            VehicleModel = vehicle.Model
        });
    }
}
