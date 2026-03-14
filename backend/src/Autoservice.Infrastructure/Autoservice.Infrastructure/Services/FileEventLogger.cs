using Autoservice.Application.Interfaces;

namespace Autoservice.Infrastructure.Services;

public class FileEventLogger : IEventLogger
{
    private readonly string _logFilePath;
    private static readonly object _lock = new();

    public FileEventLogger()
    {
        var logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        Directory.CreateDirectory(logDir);
        _logFilePath = Path.Combine(logDir, "autoservice.log");
    }

    public void Log(EventLogType type, string message)
    {
        var entry = $"[{type}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}";
        lock (_lock)
        {
            File.AppendAllText(_logFilePath, entry + Environment.NewLine);
        }
    }

    public void Info(string message) => Log(EventLogType.INFO, message);
    public void Warning(string message) => Log(EventLogType.WARNING, message);
    public void Error(string message) => Log(EventLogType.ERROR, message);
}
