using UnityEngine;

/// <summary>
/// Static variables representing the state of the game
/// </summary>
public static class GameState
{
    /// <summary>The current camera</summary>
    public static Camera Camera { get; set; }

    /// <summary>The current music box</summary>
    public static MusicBox MusicBox { get; set; }
    
    /// <summary>The time in between turns (seconds)</summary>
    public static float TurnPause { get; set; } = 2.0f;

    /// <summary>The Z-plane that pieces exist on</summary>
    public static float PieceZ { get; } = -5.0f;

    /// <summary>The player's current level</summary>
    public static int Level { get; set; } = 0;

    /// <summary>The player's highest score</summary>
    public static int HighScore { get; set; } = 0;
}
