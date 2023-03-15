using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// An object for the main menu's specific behaviors
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// Properties to set using Unity interface
    public AudioSource[] Music;
    public GameObject SettingsMenu;
    public Button PlayButton;
    public Button SettingsButton;
    public Button ExitButton;
    
    /// <summary>The save data initially loaded</summary>
    private SaveData SaveData;

    void Start()
    {
        // Load saved data
        SaveData = SaveSystem.Load();
        foreach (AudioSource source in Music) source.volume = SaveData.Volume;

        // Start the main menu music
        Music[0].Play();

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

        // Create new save data
        SaveData saveData = new SaveData();
        saveData.HighScore = SaveData.HighScore;
        saveData.Volume = Music[0].volume;

        // Write the save data to a file
        SaveSystem.Save(saveData);
    }

    /// <summary>Starts a new game</summary>
    public void Play() => SceneManager.LoadScene("Game");

    /// <summary>Opens the settings</summary>
    public void OpenSettings() => SettingsMenu.SetActive(true);

    /// <summary>Exits the application</summary>
    public void Exit() => Application.Quit();
}
