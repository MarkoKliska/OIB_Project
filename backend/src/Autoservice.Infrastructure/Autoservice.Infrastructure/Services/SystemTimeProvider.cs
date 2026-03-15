using Autoservice.Application.Interfaces;

namespace Autoservice.Infrastructure.Services;

public class SystemTimeProvider : ITimeProvider
{
    public DateTime Now => DateTime.Now;
}
