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
        GameState.GameBoard.AddPiece<King>(true, new Vector2Int(3, 7));
        GameState.GameBoard.AddPiece<Queen>(true, new Vector2Int(4, 7));
        GameState.GameBoard.AddPiece<Bishop>(true, new Vector2Int(2, 7));
        GameState.GameBoard.AddPiece<Bishop>(true, new Vector2Int(5, 7));
        GameState.GameBoard.AddPiece<Knight>(true, new Vector2Int(1, 7));
        GameState.GameBoard.AddPiece<Knight>(true, new Vector2Int(6, 7));
        GameState.GameBoard.AddPiece<Rook>(true, new Vector2Int(0, 7));
        GameState.GameBoard.AddPiece<Rook>(true, new Vector2Int(7, 7));
        GameState.GameBoard.AddPiece<Pawn>(true, new Vector2Int(0, 6));
        GameState.GameBoard.AddPiece<Pawn>(true, new Vector2Int(1, 6));
        GameState.GameBoard.AddPiece<Pawn>(true, new Vector2Int(2, 6));
        GameState.GameBoard.AddPiece<Pawn>(true, new Vector2Int(3, 6));
        GameState.GameBoard.AddPiece<Pawn>(true, new Vector2Int(4, 6));
        GameState.GameBoard.AddPiece<Pawn>(true, new Vector2Int(5, 6));
        GameState.GameBoard.AddPiece<Pawn>(true, new Vector2Int(6, 6));
        GameState.GameBoard.AddPiece<Pawn>(true, new Vector2Int(7, 6));
        GameState.GameBoard.AddPiece<King>(false, new Vector2Int(3, 0));
        GameState.GameBoard.AddPiece<Queen>(false, new Vector2Int(4, 0));
        GameState.GameBoard.AddPiece<Bishop>(false, new Vector2Int(2, 0));
        GameState.GameBoard.AddPiece<Bishop>(false, new Vector2Int(5, 0));
        GameState.GameBoard.AddPiece<Knight>(false, new Vector2Int(1, 0));
        GameState.GameBoard.AddPiece<Knight>(false, new Vector2Int(6, 0));
        GameState.GameBoard.AddPiece<Rook>(false, new Vector2Int(0, 0));
        GameState.GameBoard.AddPiece<Rook>(false, new Vector2Int(7, 0));
        GameState.GameBoard.AddPiece<Pawn>(false, new Vector2Int(0, 1));
        GameState.GameBoard.AddPiece<Pawn>(false, new Vector2Int(1, 1));
        GameState.GameBoard.AddPiece<Pawn>(false, new Vector2Int(2, 1));
        GameState.GameBoard.AddPiece<Pawn>(false, new Vector2Int(3, 1));
        GameState.GameBoard.AddPiece<Pawn>(false, new Vector2Int(4, 1));
        GameState.GameBoard.AddPiece<Pawn>(false, new Vector2Int(5, 1));
        GameState.GameBoard.AddPiece<Pawn>(false, new Vector2Int(6, 1));
        GameState.GameBoard.AddPiece<Pawn>(false, new Vector2Int(7, 1));
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
    public Piece CreatePiece<T>(bool white) where T : Piece => CreatePiece(typeof(T), white);

    /// <summary>Creates a new instance of a piece</summary>
    /// <param name="type">The type of piece to create</param>
    /// <param name="white">True if the piece is white</param>
    /// <returns>A Piece</returns>
    public Piece CreatePiece(Type type, bool white)
    {
        int prefab;
        if (type == typeof(King)) prefab = 0;
        else if (type == typeof(Queen)) prefab = 2;
        else if (type == typeof(Bishop)) prefab = 4;
        else if (type == typeof(Knight)) prefab = 6;
        else if (type == typeof(Rook)) prefab = 8;
        else if (type == typeof(Pawn)) prefab = 10;
        else throw new Exception($"Piece Type {type} not recognized");
        if (!white) prefab++;
        GameObject obj = Instantiate(PiecePrefabs[prefab]);
        Piece piece = (Piece) obj.GetComponent(type);
        piece.IsPlayerPiece = !white;
        piece.Board = GameState.GameBoard;
        return piece;
    }
}
