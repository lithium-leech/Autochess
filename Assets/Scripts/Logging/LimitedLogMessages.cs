using UnityEngine;
/// <summary>
/// Contains a collection of logs with limited capacity.
/// Older logs are deleted as newer logs come in.
/// </summary>
public class LimitedLogMessages
{
    private LimitedStringBuilder PlainTextBuilder { get; }
    private LimitedStringBuilder RichTextBuilder { get; }

    /// <summary>Creates a new LimitedLogMessages</summary>
    /// <param name="maxLogs">The maximum number of logs to hold</param>
    public LimitedLogMessages(int maxLogs)
    {
        PlainTextBuilder = new LimitedStringBuilder(maxLogs);
        RichTextBuilder = new LimitedStringBuilder(maxLogs);
    }

    /// <summary>Adds a new log to the collection</summary>
    /// <param name="log">The log to add</param>
    public void AddLog(Log log)
    {
        // Add the plain text
        string plain = log.ToString();
        PlainTextBuilder.AppendLine(plain);
        
        // Add the rich text
        string rich = log.Level switch
        {
            LogType.Error => $"<color=red>{log}</color>",
            LogType.Assert => $"<color=red>{log}</color>",
            LogType.Exception => $"<color=red>{log}</color>",
            LogType.Warning => $"<color=yellow>{log}</color>",
            _ => $"<color=white>{log}</color>",
        };
        RichTextBuilder.AppendLine(rich);
    }

    /// <summary>Returns all of the log messages as plain text</summary>
    /// <returns>A string</returns>
    public string PlainText() => PlainTextBuilder.ToString();

    /// <summary>Returns all of the log messages in rich-text format</summary>
    /// <returns>A rich-text formatted string</returns>
    public string RichText() => RichTextBuilder.ToString();
}
