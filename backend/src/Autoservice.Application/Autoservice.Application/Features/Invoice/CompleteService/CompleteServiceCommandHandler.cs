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
    IBillingService billingService,
    IEventLogger logger
) : IRequestHandler<CompleteServiceCommand, Result<ServiceInvoiceResponseDto>>
{
    public async Task<Result<ServiceInvoiceResponseDto>> Handle(
        CompleteServiceCommand command,
        CancellationToken ct)
    {
        logger.Info($"Mechanic '{command.MechanicName}' attempting to complete service for vehicle ID: {command.VehicleId}");

        var vehicle = await vehicles.GetByIdAsync(command.VehicleId, ct);

        if (vehicle is null)
        {
            logger.Error($"Vehicle not found. ID: {command.VehicleId}");
            return Result<ServiceInvoiceResponseDto>.Failure("Vehicle not found.");
        }

        if (vehicle.IsServiced)
        {
            logger.Warning($"Vehicle '{vehicle.LicensePlate}' has already been serviced.");
            return Result<ServiceInvoiceResponseDto>.Failure("Vehicle has already been serviced.");
        }

        var finalAmount = billingService.CalculateFinalAmount(vehicle.EstimatedPrice);
        logger.Info($"Billing calculated for '{vehicle.LicensePlate}': estimated {vehicle.EstimatedPrice}, final {finalAmount}");

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

        logger.Info($"Service completed and invoice issued for vehicle '{vehicle.LicensePlate}'. Total: {finalAmount}");

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
