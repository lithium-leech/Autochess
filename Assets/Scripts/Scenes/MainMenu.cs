using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// An object for the main menu's specific behaviors
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// Properties to set using Unity interface
    public GameObject SettingsMenu;
    public Button PlayButton;
    public Button SettingsButton;
    public Button ExitButton;
    
    void Start()
    {
        // Start the main menu music
        GameState.MusicBox.PlayMusic(SongName.Menu);

        // Add button handlers
        PlayButton.onClick.AddListener(Play);
        SettingsButton.onClick.AddListener(OpenSettings);
        ExitButton.onClick.AddListener(Exit);
    }

    private void OnDestroy()
    {
        // Remove button handlers
        PlayButton.onClick.RemoveAllListeners();
        SettingsButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();
    }

    /// <summary>Starts a new game</summary>
    public void Play() => SceneManager.LoadScene("Game");

    /// <summary>Opens the settings</summary>
    public void OpenSettings() => SettingsMenu.SetActive(true);

    /// <summary>Exits the application</summary>
    public void Exit() => Application.Quit();
}
