using Autoservice.Domain.Entities;

namespace Autoservice.Domain.Repositories;

public interface IVehicleRepository
{
    Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<Vehicle>> GetUnservicedAsync(CancellationToken ct = default);
    Task<int> GetActiveCountAsync(CancellationToken ct = default);
    Task AddAsync(Vehicle vehicle, CancellationToken ct = default);
    Task UpdateAsync(Vehicle vehicle, CancellationToken ct = default);
}
