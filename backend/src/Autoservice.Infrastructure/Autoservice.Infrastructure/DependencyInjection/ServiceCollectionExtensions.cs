using Autoservice.Application.Authentication;
using Autoservice.Application.Interfaces;
using Autoservice.Infrastructure.Persistence.Contexts;
using Autoservice.Infrastructure.Persistence.UnitOfWork;
using Autoservice.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Autoservice.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AutoserviceDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(AutoserviceDbContext).Assembly.FullName)
            ));

        //services.AddScoped<IUserRepository, UserRepository>();
        //services.AddScoped<IVehicleRepository, VehicleRepository>();
        //services.AddScoped<IServiceInvoiceRepository, ServiceInvoiceRepository>();

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
