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
    public Button PlayButton;
    public Button ExitButton;

    void Start()
    {
        // Start the main menu music
        Music[0].Play();
        PlayButton.onClick.AddListener(Play);
        ExitButton.onClick.AddListener(Exit);
    }

    /// <summary>Starts a new game</summary>
    public void Play() => SceneManager.LoadScene("Game");

    /// <summary>Exits the application</summary>
    public void Exit() => Application.Quit();
}
