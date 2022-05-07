using System;
using UnityEngine;

/// <summary>
/// The main game object, responsible for managing
/// the state of the game
/// </summary>
public class Game : MonoBehaviour
{
    /// <summary>Prefabs for game pieces</summary>
    public GameObject[] PiecePrefabs;

    private void Start()
    {
        // Set game states
        GameState.FightStarted = false;
        GameState.InFight = false;
        GameState.FightOver = false;
        GameState.Victory = false;

        // Create the game boards
        GameState.GameBoard = new Board(this, 8, 8, -4.0f, 1.0f);
        GameState.SideBoard = new Board(this, 8, 3, -4.0f, -4.0f);

        // Create a sample setup
        GameState.GameBoard.AddPiece<Pawn>(true, new Vector2Int(3, 6));
        GameState.GameBoard.AddPiece<Pawn>(false, new Vector2Int(4, 1));
        GameState.GameBoard.AddPiece<Queen>(true, new Vector2Int(7, 6));
        GameState.GameBoard.AddPiece<Queen>(false, new Vector2Int(0, 1));
    }

    private float timeWaited = 0;
    private bool whiteTurn = true;

    private void Update()
    {
        // Start a fight once it's been switched on
        if (GameState.FightStarted && !GameState.InFight)
        {
            GameState.InFight = true;
            GameState.FightStarted = false;
            timeWaited = 0;
            whiteTurn = true;
        }

        // Run fight operations
        if (GameState.InFight)
        {
            timeWaited += Time.deltaTime;
            if (timeWaited > GameState.TurnPause)
            {
                // Move the current player's pieces
                if (whiteTurn)
                {
                    foreach (Piece piece in GameState.GameBoard.EnemyPieces) piece.TakeTurn();
                }
                else
                {
                    foreach (Piece piece in GameState.GameBoard.PlayerPieces) piece.TakeTurn();
                }

                // Check if the battle is over
                if (GameState.GameBoard.PlayerPieces.Count < 1)
                {
                    GameState.FightOver = true;
                    GameState.InFight = false;
                }
                else if (GameState.GameBoard.EnemyPieces.Count < 1)
                {
                    GameState.Victory = true;
                    GameState.FightOver = true;
                    GameState.InFight = false;
                }

                // Go to the next turn
                whiteTurn = !whiteTurn;
                timeWaited = 0;
            }
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

    /// <summary>Creates a new instance of a piece</summary>
    /// <typeparam name="T">The type of piece to create</typeparam>
    /// <param name="white">True if the piece is white</param>
    /// <returns>A Piece</returns>
    public Piece CreatePiece<T>(bool white) where T : Piece
    {
        int prefab;
        if (typeof(T) == typeof(Queen)) prefab = 2;
        else if (typeof(T) == typeof(Pawn)) prefab = 10;
        else throw new Exception($"Piece Type {nameof(T)} not recognized");
        if (!white) prefab++;
        GameObject obj = Instantiate(PiecePrefabs[prefab]);
        Piece piece = obj.GetComponent<T>();
        piece.IsPlayerPiece = !white;
        piece.Board = GameState.GameBoard;
        return piece;
    }
}
