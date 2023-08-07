using UnityEngine;

/// <summary>
/// Initializes a new scene
/// </summary>
public class SceneLauncher : MonoBehaviour
{
    /// Properties to set using Unity interface
    public Camera Camera;
    public MusicBox MusicBox;

    void Awake()
    {
        // Load saved data
        SaveData saveData = SaveSystem.Load();

        // Initialize the game state
        GameState.Camera = Camera;
        GameState.MusicBox = MusicBox;
        GameState.MusicBox.Volume = saveData.Volume;
        GameState.HighScore = saveData.HighScore;
    }

    void OnDestroy()
    {
        // Create new save data
        SaveData saveData = new()
        {
            HighScore = GameState.HighScore,
            Volume = GameState.MusicBox.Volume
        };

        // Write the save data to a file
        SaveSystem.Save(saveData);
    }
}
