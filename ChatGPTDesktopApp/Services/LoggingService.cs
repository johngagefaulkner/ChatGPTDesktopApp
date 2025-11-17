using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Windows.Storage;

namespace ChatGPTDesktopApp.Services;

internal static class LoggingService
{
    private static string AppDataDirectory => ApplicationData.GetDefault().LocalFolder.Path;
    private const string LogDirectoryName = "Logs";
    private static string LogDirectoryPath => Path.Combine(AppDataDirectory, LogDirectoryName);
    private static string LogFileName => $"Log_{DateTime.Now:yyyyMMdd_HHmmss}.log";
    private static string LogFilePath => Path.Combine(LogDirectoryPath, LogFileName);

    /// <summary>
    /// Gets the file path of the current log file.
    /// </summary>
    /// <returns>The file path of the current log file.</returns>
    public static string GetLogFilePath() => LogFilePath;

    private static void EnsureLogDirectoryExists()
    {
        if (!Directory.Exists(LogDirectoryPath))
        {
            Directory.CreateDirectory(LogDirectoryPath);
        }
    }

    private static void LogToFile(string level, string message)
    {
        try
        {
            EnsureLogDirectoryExists();
            var logEntry = $"[{level}] {DateTime.Now:s}: {message}{Environment.NewLine}";
            File.AppendAllText(LogFilePath, logEntry);
        }

        catch (Exception ex)
        {
            // If logging fails, there's not much we can do. Consider handling this silently.
            System.Diagnostics.Debug.WriteLine($"Logging failed: {ex}");
        }
    }

    /// <summary>
    /// Logs an informational message to the log file.
    /// </summary>
    /// <param name="message">The informational message to log.</param>
    public static void LogInfo(string message) => LogToFile("INFO", message);
    public static void LogWarning(string message) => LogToFile("WARNING", message);
    public static void LogError(string message) => LogToFile("ERROR", message);
    public static void LogError(string message, Exception ex)
    {
        var fullMessage = $"{message} Exception: {ex}";
        LogToFile("ERROR", fullMessage);
    }
    public static void LogError(Exception logEx) => LogToFile("ERROR", $"Exception: {logEx}");
}
