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

    /// <summary>The player's current level</summary>
    public static int Level { get; set; } = 0;

    /// <summary>The player's highest score</summary>
    public static int HighScore { get; set; }

    /// <summary>The map to load when a game starts</summary>
    public static AssetGroup.Map StartMap { get; set; }
    
    /// <summary>The set to load when a game starts</summary>
    public static AssetGroup.Set StartSet { get; set; }

    /// <summary>True if the player's color is white</summary>
    public static bool IsPlayerWhite { get; set; } = false;

    /// <summary>True during rounds where at least one piece has moved</summary>
    public static bool IsActiveRound { get; set; }
    
    /// <summary>The number of rounds to complete for a piece upgrade to be offered</summary>
    public static int PieceRounds { get; } = 1;

    /// <summary>The number of rounds to complete for a power upgrade to be offered</summary>
    public static int PowerRounds { get; } = 5;

    /// <summary>The number of rounds to complete for a map upgrade to be offered</summary>
    public static int MapRounds { get; } = 20;

    /// <summary>The Z-plane that background board tiles exist on</summary>
    public static Vector3 BackboardZ { get; } = new Vector3(0.0f, 0.0f, -3.0f);

    /// <summary>The Z-plane that board tiles exist on</summary>
    public static Vector3 BoardZ { get; } = new Vector3(0.0f, 0.0f, -4.0f);

    /// <summary>The Z-plane that terrain exists on</summary>
    public static Vector3 TerrainZ { get; } = new Vector3(0.0f, 0.0f, -5.0f);

    /// <summary>The Z-plane that stationary pieces exist on</summary>
    public static Vector3 StillPieceZ { get; } = new Vector3(0.0f, 0.0f, -6.0f);

    /// <summary>The Z-plane that highlights exist on</summary>
    public static Vector3 HighlightZ { get; } = new Vector3(0.0f, 0.0f, -7.0f);

    /// <summary>The Z-plane that moving pieces exist on</summary>
    public static Vector3 MovingPieceZ { get; } = new Vector3(0.0f, 0.0f, -8.0f);

    public static Vector3 ShadowZone { get; } = new Vector3(100.0f, 100.0f, 100.0f);
}
