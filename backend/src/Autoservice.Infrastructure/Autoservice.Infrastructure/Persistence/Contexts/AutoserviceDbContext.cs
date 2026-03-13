using Autoservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autoservice.Infrastructure.Persistence.Contexts;

public class AutoserviceDbContext : DbContext
{
    public AutoserviceDbContext(DbContextOptions<AutoserviceDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<ServiceInvoice> ServiceInvoices => Set<ServiceInvoice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutoserviceDbContext).Assembly);
        base.OnModelCreating(modelBuilder);   
    }
}
