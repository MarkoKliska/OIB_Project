using Autoservice.Domain.Entities;
using Autoservice.Domain.Repositories;
using Autoservice.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Autoservice.Infrastructure.Persistence.Repositories;

public class VehicleRepository(AutoserviceDbContext context) : IVehicleRepository
{
    public async Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Vehicles.FindAsync([id], cancellationToken);

    public async Task<Vehicle?> GetByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default)
        => await context.Vehicles.FirstOrDefaultAsync(v => v.LicensePlate == licensePlate, cancellationToken);

    public async Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.Vehicles.Include(v => v.ServiceInvoice).ToListAsync(cancellationToken);

    public async Task<IEnumerable<Vehicle>> GetUnservicedAsync(CancellationToken cancellationToken = default)
        => await context.Vehicles.Where(v => !v.IsServiced).ToListAsync(cancellationToken);

    public async Task<int> GetActiveCountAsync(CancellationToken cancellationToken = default)
        => await context.Vehicles.CountAsync(v => !v.IsServiced, cancellationToken);

    public async Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
        => await context.Vehicles.AddAsync(vehicle, cancellationToken);

    public Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        context.Vehicles.Update(vehicle);
        return Task.CompletedTask;
    }
}
