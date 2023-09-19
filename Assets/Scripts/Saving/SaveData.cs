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

    /// <summary>The map to load when the game starts</summary>
    public AssetGroup.Map StartMap = AssetGroup.Map.Classic;

    /// <summary>The set to load when the game starts</summary>
    public AssetGroup.Set StartSet = AssetGroup.Set.Western;
}
