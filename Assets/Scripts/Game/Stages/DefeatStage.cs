using UnityEngine.SceneManagement;

/// <summary>
/// The stage of the game after the player loses
/// </summary>
public class DefeatStage : IStage
{
    /// <summary>Creates a new instance of a DefeatStage</summary>
    /// <param name="game">The Game to run the DefeatStage in</param>
    public DefeatStage(Game game)
    {
        Game = game;
    }

    /// <summary>The Game this is being run in</summary>
    private Game Game { get; set; }

    public void Start()
    {
        // Stop all music
        GameState.MusicBox.StopMusic();

        // Show the game over screen
        MenuManager.AddActiveMenu(Game.GameOverMenu);

        // Add button listeners
        Game.NewGameButton.onClick.AddListener(NewGame);
        Game.EndGameButton.onClick.AddListener(EndGame);
        Game.RetryButton.onReward += RetryLevel;
    }

    public void During() {}

    public void End()
    {
        // Remove button Listeners
        Game.NewGameButton.onClick.RemoveAllListeners();
        Game.EndGameButton.onClick.RemoveAllListeners();
        Game.RetryButton.onReward -= RetryLevel;

        // Hide the game over menu
        MenuManager.RemoveActiveMenu(Game.GameOverMenu);
    }

    /// <summary>Starts the level over</summary>
    public void RetryLevel() => Game.NextStage = new PlanningStage(Game);

    /// <summary>Starts the game over</summary>
    public void NewGame() => SceneManager.LoadScene("Game");
     
    /// <summary>Returns to the main menu</summary>
    public void EndGame() => SceneManager.LoadScene("MainMenu");
}
