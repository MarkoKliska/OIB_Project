namespace Autoservice.Application.Interfaces;

public interface IBillingService
{
    decimal CalculateFinalAmount(decimal estimatedPrice);
}
