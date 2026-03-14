using Autoservice.Application.Interfaces;

namespace Autoservice.Infrastructure.Services;

public class AfternoonBillingService : IBillingService
{
    public decimal CalculateFinalAmount(decimal estimatedPrice)
    {
        return estimatedPrice * 1.10m;
    }
}
