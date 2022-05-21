using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Contains actions for menu buttons
/// </summary>
public class Menu : MonoBehaviour
{
    /// <summary>Starts a new game</summary>
    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    /// <summary>Starts the next battle sequence</summary>
    public void Fight()
    {
        GameState.PlanningOver = true;
        GameState.FightStarted = true;
    }

    /// <summary>Exits the game and goes back to the main menu</summary>
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    /// <summary>Exits the application</summary>
    public void Exit()
    {
        Application.Quit();
    }
}
