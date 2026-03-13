using Autoservice.Domain.Entities;
using Autoservice.Domain.Repositories;
using Autoservice.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Autoservice.Infrastructure.Persistence.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly AutoserviceDbContext _context;
    public VehicleRepository(AutoserviceDbContext context) => _context = context;

    public async Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _context.Vehicles.FindAsync([id], ct);

    public async Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken ct = default) =>
        await _context.Vehicles.Include(v => v.ServiceInvoice).ToListAsync(ct);

    public async Task<IEnumerable<Vehicle>> GetUnservicedAsync(CancellationToken ct = default) =>
        await _context.Vehicles.Where(v => !v.IsServiced).ToListAsync(ct);

    public async Task<int> GetActiveCountAsync(CancellationToken ct = default) =>
        await _context.Vehicles.CountAsync(v => !v.IsServiced, ct);

    public async Task AddAsync(Vehicle vehicle, CancellationToken ct = default) =>
        await _context.Vehicles.AddAsync(vehicle, ct);

    public Task UpdateAsync(Vehicle vehicle, CancellationToken ct = default)
    {
        _context.Vehicles.Update(vehicle);
        return Task.CompletedTask;
    }
}
