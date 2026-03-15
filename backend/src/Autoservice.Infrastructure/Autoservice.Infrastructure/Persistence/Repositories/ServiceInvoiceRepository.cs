using Autoservice.Domain.Entities;
using Autoservice.Domain.Repositories;
using Autoservice.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Autoservice.Infrastructure.Persistence.Repositories;

public class ServiceInvoiceRepository : IServiceInvoiceRepository
{
    private readonly AutoserviceDbContext _context;
    public ServiceInvoiceRepository(AutoserviceDbContext context) => _context = context;

    public async Task<ServiceInvoice?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _context.ServiceInvoices
            .Include(si => si.Vehicle)
            .Include(si => si.Mechanic)
            .FirstOrDefaultAsync(si => si.Id == id, ct);

    public async Task<IEnumerable<ServiceInvoice>> GetAllAsync(CancellationToken ct = default) =>
        await _context.ServiceInvoices
            .Include(si => si.Vehicle)
            .Include(si => si.Mechanic)
            .OrderByDescending(si => si.IssuedAt)
            .ToListAsync(ct);

    public async Task AddAsync(ServiceInvoice invoice, CancellationToken ct = default) =>
        await _context.ServiceInvoices.AddAsync(invoice, ct);
}
