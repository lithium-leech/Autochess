using TMPro;
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

        // Show the final score
        Game.ScoreText.GetComponent<TextMeshProUGUI>().text = Game.Level.ToString();
        Game.HighScoreText.GetComponent<TextMeshProUGUI>().text = Game.HighScore.ToString();
        Game.GameOverMenu.SetActive(true);

        // Add button listeners
        Game.StartOverButton.onClick.AddListener(StartOver);
    }

    public void During()
    {

    }

    public void End()
    {
        // Remove button Listeners
        Game.StartOverButton.onClick.RemoveAllListeners();

        // Hide the game over menu
        Game.GameOverMenu.SetActive(false);
    }

    /// <summary>Starts the game over</summary>
    public void StartOver() => SceneManager.LoadScene("Game");
}
