public static class GameState
{
    /// <summary>True when the fight is started</summary>
    public volatile static bool FightStarted;
    
    /// <summary>True if the fight is currently running</summary>
    public volatile static bool InFight;

    /// <summary>True when the fight is over/summary>
    public volatile static bool FightOver;

    /// <summary>True if the fight was won</summary>
    public volatile static bool Victory;

    /// <summary>The time in between turns (seconds)</summary>
    public static float TurnPause = 2;

    /// <summary>The board that fights take place on</summary>
    public static Board GameBoard { get; set; }

    /// <summary>The board that holds the player's available pieces</summary>
    public static Board SideBoard { get; set; }

    /// <summary>Starts the next battle sequence</summary>
    public static void StartFight() => FightStarted = true;
}
