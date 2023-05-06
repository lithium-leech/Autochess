using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the in-game log view
/// </summary>
public class LogManager : MonoBehaviour
{
    /// Properties to set using Unity interface
    public GameObject ScreenCover;
    public BasicButton LogButton;
    public BasicButton CopyButton;
    public BasicButton CloseButton;
    public TextMeshProUGUI LogText;
    public ScrollRect Console;

    /// <summary>All of the log messages</summary>
    private static LimitedLogMessages Logs { get; } = new LimitedLogMessages(1000);

    void OnEnable() => Application.logMessageReceived += HandleLog;

    void OnDisable() => Application.logMessageReceived -= HandleLog;

    /// <summary>Handles incoming log messages</summary>
    /// <param name="logString">The log string</param>
    /// <param name="stackTrace">The stack trace</param>
    /// <param name="type">The log type</param>
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        Logs.AddLog(new Log(type, logString, Time.time));
        LogText.text = Logs.RichText();
    }

    /// <summary>Shows the log window</summary>
    public void ShowLogs()
    {
        // Show the log view
        ScreenCover.SetActive(true);

        // Disable log button
        LogButton.SetEnabled(false);

        // Scroll to the bottom of the log view
        Console.verticalNormalizedPosition = 0f;
    }

    /// <summary>Close the log window</summary>
    public void CloseLogs()
    {
        // Hide the log view
        ScreenCover.SetActive(false);

        // Enable log button
        LogButton.SetEnabled(true);
    }

    /// <summary>Copy the logs into the clipboard</summary>
    public void CopyLogs() => GUIUtility.systemCopyBuffer = Logs.PlainText();
}
