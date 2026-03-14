namespace Autoservice.Application.Interfaces;

public enum EventLogType
{
    INFO,
    WARNING,
    ERROR
}

public interface IEventLogger
{
    void Log(EventLogType type, string message);
    void Info(string message);
    void Warning(string message);
    void Error(string message);
}
