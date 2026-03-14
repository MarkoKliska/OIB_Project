using Autoservice.Application.Interfaces;

namespace Autoservice.Infrastructure.Services;

public class MorningBillingService : IBillingService
{
    public decimal CalculateFinalAmount(decimal estimatedPrice)
    {
        return estimatedPrice * 0.85m;
    }
}
