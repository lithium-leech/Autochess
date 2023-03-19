using System.Text;
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
    public TextMeshProUGUI LogText;

    /// <summary>All of the log messages</summary>
    private static StringBuilder LogMessages { get; } = new StringBuilder(); 

    void OnEnable() => Application.logMessageReceived += HandleLog;

    void OnDisable() => Application.logMessageReceived -= HandleLog;

    /// <summary>Handles incoming log messages</summary>
    /// <param name="logString">The log string</param>
    /// <param name="stackTrace">The stack trace</param>
    /// <param name="type">The log type</param>
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Log)
        {
            LogMessages.AppendLine(logString);
            LogText.text = LogMessages.ToString();
        }
    }

    private void Start() => LogButton.onClick.AddListener(ShowLogs);

    private void OnDestroy() => LogButton.onClick.RemoveAllListeners();

    /// <summary>Toggles the log window</summary>
    public void ShowLogs() => MenuCover.SetActive(!MenuCover.activeInHierarchy);
}
