using Autoservice.Domain.Entities;

namespace Autoservice.Domain.Repositories;

public interface IServiceInvoiceRepository
{
    Task<ServiceInvoice?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<ServiceInvoice>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(ServiceInvoice invoice, CancellationToken ct = default);
}