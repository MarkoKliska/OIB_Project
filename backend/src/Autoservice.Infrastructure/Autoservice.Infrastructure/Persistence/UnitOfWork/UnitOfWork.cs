using Autoservice.Infrastructure.Persistence.Contexts;
using Autoservice.Application.Interfaces;

namespace Autoservice.Infrastructure.Persistence.UnitOfWork;

public class UnitOfWork(AutoserviceDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}
