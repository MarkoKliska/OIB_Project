using Autoservice.Application.Interfaces;

namespace Autoservice.Infrastructure.Services;

public class BillingServiceFactory
{
    private readonly MorningBillingService _morningBilling;
    private readonly AfternoonBillingService _afternoonBilling;

    public BillingServiceFactory(
        MorningBillingService morningBilling,
        AfternoonBillingService afternoonBilling)
    {
        _morningBilling = morningBilling;
        _afternoonBilling = afternoonBilling;
    }

    public IBillingService GetForCurrentShift()
    {
        var hour = DateTime.Now.Hour;

        if (hour >= 8 && hour < 12)
            return _morningBilling;

        return _afternoonBilling;
    }
}
