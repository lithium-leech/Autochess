using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// An object for the main menu's specific behaviors
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// Properties to set using Unity interface
    public GameObject Menu;
    public GameObject SettingsMenu;
    
    void Start()
    {
        // Initialize the menu manager
        MenuManager.Initialize(Menu);

        // Start the main menu music
        GameState.MusicBox.PlayMusic(SongName.Menu);
    }

    /// <summary>Starts a new game</summary>
    public void Play() => SceneManager.LoadScene("Game");

    /// <summary>Opens the settings</summary>
    public void OpenSettings() => MenuManager.OpenOverlay(SettingsMenu);

    /// <summary>Exits the application</summary>
    public void Exit() => Application.Quit();
}
