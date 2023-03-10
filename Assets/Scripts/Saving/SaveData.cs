/// <summary>
/// Holds information to retain when the game is closed
/// </summary>
[System.Serializable]
public class SaveData
{
    /// <summary>The player's highest score</summary>
    public int HighScore = 0;

    /// <summary>The player's selected volume level</summary>
    public float Volume = 0.7f;
}
