using UnityEngine;

/// <summary>
/// An object for the main menu's specific behaviors
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// Properties to set using Unity interface
    public AudioSource[] Music;

    void Start()
    {
        // Start the main menu music
        Music[0].Play();
    }
}
