using UnityEngine;

/// <summary>
/// Represents a single log message
/// </summary>
public class Log
{
    /// <summary>The log level</summary>
    public LogType Level { get; }

    /// <summary>The log message</summary>
    public string Message { get; }

    /// <summary>The log time</summary>
    public float Time { get; }

    /// <summary>Creates a new log</summary>
    /// <param name="level">The log level</param>
    /// <param name="message">The log message</param>
    /// <param name="time">The log time</param>
    public Log(LogType level, string message, float time)
    {
        Level = level;
        Message = message;
        Time = time;
    }

    public override string ToString() => $"{Time:0.0} | {Level} | {Message}";
}
