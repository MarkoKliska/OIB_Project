namespace Autoservice.Application.Interfaces;

public interface ITimeProvider
{
    DateTime Now { get; }
}
