/// <summary>
/// Holds a static collection of global game state variables
/// </summary>
public static class GameState
{
    /// <summary>True when the planning phase is started</summary>
    public volatile static bool PlanningStarted;

    /// <summary>True if the planning phase is currently running</summary>
    public volatile static bool InPlanning;

    /// <summary>True when the planning phase is over/summary>
    public volatile static bool PlanningOver;
    
    /// <summary>True when the fight is started</summary>
    public volatile static bool FightStarted;
    
    /// <summary>True if the fight is currently running</summary>
    public volatile static bool InFight;

    /// <summary>True when the fight is over/summary>
    public volatile static bool FightOver;

    /// <summary>True if the fight was won</summary>
    public volatile static bool Victory;

    /// <summary>True if the player should be prompted to concede</summary>
    public volatile static bool ConcedeStarted;

    /// <summary>True if the player is being prompted to concede</summary>
    public volatile static bool InConcede;

    /// <summary>The current level</summary>
    public static int Level { get; set; } = 0;

    /// <summary>The player's highest level achieved</summary>
    public static int HighScore { get; set; } = 0;

    /// <summary>The number of rounds until the player should be prompted to concede</summary>
    public static int RoundsToConcede { get; set; } = 0;

    /// <summary>The number of rounds that have occured with no pieces moving</summary>
    public static int RoundsStatic { get; set; } = 0;

    /// <summary>The time in between turns (seconds)</summary>
    public static float TurnPause { get; } = 2;

    /// <summary>The Z-plane that pieces exist on</summary>
    public static float PieceZ { get; } = -1;

    /// <summary>The board that fights take place on</summary>
    public static Board GameBoard { get; set; }

    /// <summary>The board that holds the player's available pieces</summary>
    public static Board SideBoard { get; set; }
}
