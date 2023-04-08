using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the in-game log view
/// </summary>
public class LogManager : MonoBehaviour
{
    /// Properties to set using Unity interface
    public GameObject MenuCover;
    public Button LogButton;
    public Button CopyButton;
    public TextMeshProUGUI LogText;

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

    /// <summary>Toggles the log window</summary>
    public void ShowLogs()
    {
        MenuCover.SetActive(!MenuCover.activeInHierarchy);
        CopyButton.gameObject.SetActive(!CopyButton.gameObject.activeInHierarchy);
        CopyButton.interactable = !CopyButton.interactable;
    }

    public void CopyLogs() => GUIUtility.systemCopyBuffer = Logs.PlainText();
}
