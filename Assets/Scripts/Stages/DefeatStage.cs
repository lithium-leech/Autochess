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
        Game.MusicBox.StopMusic();

        // Show the game over screen
        Game.GameOverMenu.SetActive(true);

        // Add button listeners
        Game.EndGameButton.onClick.AddListener(EndGame);
        Game.RetryButton.onReward += RetryLevel;
    }

    public void During()
    {

    }

    public void End()
    {
        // Remove button Listeners
        Game.EndGameButton.onClick.RemoveAllListeners();
        Game.RetryButton.onReward -= RetryLevel;

        // Hide the game over menu
        Game.GameOverMenu.SetActive(false);
    }

    /// <summary>Starts the level over</summary>
    public void RetryLevel() => Game.NextStage = new PlanningStage(Game);

    /// <summary>Starts the game over</summary>
    public void EndGame() => SceneManager.LoadScene("Main Menu");
}
