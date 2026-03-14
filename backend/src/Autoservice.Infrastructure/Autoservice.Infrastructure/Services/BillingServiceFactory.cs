using Autoservice.Application.Interfaces;

namespace Autoservice.Infrastructure.Services;

public class BillingServiceFactory
{
    private readonly MorningBillingService _morningBilling;
    private readonly AfternoonBillingService _afternoonBilling;
    private readonly ITimeProvider _timeProvider;

    public BillingServiceFactory(
        MorningBillingService morningBilling,
        AfternoonBillingService afternoonBilling,
        ITimeProvider timeProvider)
    {
        _morningBilling = morningBilling;
        _afternoonBilling = afternoonBilling;
        _timeProvider = timeProvider;
    }

    public IBillingService GetForCurrentShift()
    {
        var hour = _timeProvider.Now.Hour;

        // First shift:  08:00 - 12:00 → morning (15% discount)
        // Second shift: 12:00 - 16:00 → afternoon (10% tax)
        if (hour >= 8 && hour < 12)
            return _morningBilling;

        return _afternoonBilling;
    }
}
