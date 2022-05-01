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

    private void Start()
    {
        // Set game states
        GameState.FightStarted = false;
        GameState.InFight = false;
        GameState.FightOver = false;
        GameState.Victory = false;

        // Create the game boards
        GameState.GameBoard = new Board(8, 8, -4.0f, 1.0f);
        GameState.SideBoard = new Board(8, 3, -4.0f, -4.0f);

        // Create a sample setup
        GameState.GameBoard.Add(new Pawn(Instantiate(WhitePawn), false), new Vector2Int(3, 6));
        GameState.GameBoard.Add(new Pawn(Instantiate(BlackPawn), true), new Vector2Int(4, 1));
    }

    private void Update()
    {
        // Start a fight once it's been switched on
        if (GameState.FightStarted && !GameState.InFight)
        {
            GameState.InFight = true;
            GameState.FightStarted = false;
            Thread battle = new(Fight);
            battle.Start();
        }

        // Perform end of fight actions
        if (GameState.FightOver)
        {
            GameState.FightOver = false;
            if (GameState.Victory)
            {
                
            }
            else
            {
                
            }
        }
    }

    /// <summary>Runs the battle sequence</summary>
    private void Fight()
    {
        // Pause briefly before starting the fight
        int turnPause = 2000;
        Thread.Sleep(turnPause/4);
        bool playerTurn = false;

        // Start the battle loop (with finite finish)
        for (int i = 0; i < 100; i++)
        {
            // Move the current player's pieces
            if (playerTurn)
            {
                foreach (Piece piece in GameState.GameBoard.PlayerPieces) piece.Move();
            }
            else
            {
                foreach (Piece piece in GameState.GameBoard.EnemyPieces) piece.Move();
            }
            playerTurn = !playerTurn;
            Thread.Sleep(turnPause);

            // Check if the battle is over
            if (GameState.GameBoard.PlayerPieces.Count < 1)
            {
                break;
            }
            else if (GameState.GameBoard.EnemyPieces.Count < 1)
            {
                GameState.Victory = true;
                break;
            }
        }
        GameState.FightOver = true;
    }
}
