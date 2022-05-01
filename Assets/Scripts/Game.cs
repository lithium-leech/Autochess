using System.Threading;
using UnityEngine;

/// <summary>
/// The main game object, responsible for managing
/// the state of the game
/// </summary>
public class Game : MonoBehaviour
{
    /// <summary>Prefab for a white pawn</summary>
    public GameObject WhitePawn;

    /// <summary>Prefab for a black pawn</summary>
    public GameObject BlackPawn;

    /// <summary>The board that fights take place on</summary>
    private Board Board { get; set; }
    
    /// <summary>The board that holds the player's available pieces</summary>
    private Board Tableu { get; set; }

    private void Start()
    {    
        // Create the game boards
        Board = new Board(8, 8, -4.0f, 1.0f);
        Tableu = new Board(8, 3, -4.0f, -4.0f);

        // Create a sample setup
        Board.Add(new Pawn(Instantiate(WhitePawn), true), new Vector2Int(3, 6));
        Board.Add(new Pawn(Instantiate(BlackPawn), false), new Vector2Int(4, 1));
    }

    private void Update()
    {
        // Start a fight once it's been switched on
        if (GameManager.FightStarted && !GameManager.InFight)
        {
            GameManager.InFight = true;
            GameManager.FightStarted = false;
            Thread battle = new(Fight);
            battle.Start();
        }

        // Perform end of fight actions
        if (GameManager.FightOver)
        {
            GameManager.FightOver = false;
            if (GameManager.Victory)
            {

            }
            else
            {

            }
        }
    }

    /// <summary>Runs the battle sequence</summary>
    /// <returns>True if the battle is won</returns>
    private void Fight()
    {
        // Start the battle loop (with finite finish)
        for (int i = 0; i < 100; i++)
        {
            // Move white's pieces
            foreach (Piece piece in Board.EnemyPieces) piece.Move();
            Thread.Sleep(5000);

            // Move black's pieces
            foreach (Piece piece in Board.PlayerPieces) piece.Move();
            Thread.Sleep(5000);

            // Check if the battle is over
            if (Board.PlayerPieces.Count < 1)
            {
                break;
            }
            else if (Board.EnemyPieces.Count < 1)
            {
                GameManager.Victory = true;
                break;
            }
        }
        GameManager.FightOver = true;
    }
}

/// <summary>
/// Static variables shared by the Game
/// </summary>
public static class GameManager
{
    /// <summary>True if the fight is currently running</summary>
    public static bool InFight { get; set; } = false;
    
    /// <summary>True when the fight is started</summary>
    public static bool FightStarted { get; set; } = false;

    /// <summary>True when the fight is over/summary>
    public static bool FightOver { get; set; } = false;

    /// <summary>True if the fight was won</summary>
    public static bool Victory { get; set; } = false;
}
