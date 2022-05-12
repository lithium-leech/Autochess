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

    /// <summary>The main camera</summary>
    public Camera Camera;

    /// <summary>The Piece currently being moved around by the player</summary>
    private Piece HeldPiece { get; set; }

    private void Start()
    {
        // Set game states
        GameState.FightStarted = false;
        GameState.InFight = false;
        GameState.FightOver = false;
        GameState.Victory = false;

        // Create the game boards
        GameState.GameBoard = new Board(this, 8, 8, new Vector2(-4.0f, 1.0f));
        GameState.SideBoard = new Board(this, 8, 3, new Vector2(-4.0f, -4.0f));

        // Create a sample setup
        GameState.GameBoard.AddPiece<Pawn>(true, new Vector2Int(0, 7));
        GameState.GameBoard.AddPiece<Pawn>(true, new Vector2Int(2, 7));
        GameState.GameBoard.AddPiece<Pawn>(true, new Vector2Int(4, 6));
        GameState.GameBoard.AddPiece<Pawn>(true, new Vector2Int(7, 6));

        GameState.SideBoard.AddPiece<Pawn>(false);
        GameState.SideBoard.AddPiece<Pawn>(false);
        GameState.SideBoard.AddPiece<Pawn>(false);

        // Start off in planning phase
        GameState.InPlanningPhase = true;
    }

    private float timeWaited = 0;
    private bool whiteTurn = true;

    private void Update()
    {
        DetectFightStart();
        RunFight();
        DetectFightFinish();
        RunPlanningPhase();
    }

    /// <summary>Starts a fight once it's been switched on</summary>
    private void DetectFightStart()
    {
        if (GameState.FightStarted && !GameState.InFight)
        {
            GameState.FightStarted = false;
            GameState.InFight = true;
            GameState.InPlanningPhase = false;
            timeWaited = 0;
            whiteTurn = true;
        }
    }

    /// <summary>Runs the battle operations</summary>
    private void RunFight()
    {
        // Only run when in the fighting phase
        if (!GameState.InFight) return;

        // Pause between turns
        timeWaited += Time.deltaTime;
        if (timeWaited < GameState.TurnPause) return;
         
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

    /// <summary>Performs end of fight operations</summary>
    private void DetectFightFinish()
    {
        // Only run when the fight first ends
        if (!GameState.FightOver) return;
        GameState.FightOver = false;
         
        // Determine if the battle was won
        if (GameState.Victory)
        {

        }
        else
        {

        }
    }

    /// <summary>Handles player inputs during the planning phase</summary>
    private void RunPlanningPhase()
    {
        // Only run when in the planning phase
        if (!GameState.InPlanningPhase) return;

        // Get the current mouse position
        Vector3 position = Camera.ScreenToWorldPoint(Input.mousePosition);
        position.z = GameState.PieceZ;

        // Check if the position is inside a board
        Board board = null;
        if (GameState.GameBoard.CornerBL.x <= position.x &&
            position.x <= GameState.GameBoard.CornerTR.x &&
            GameState.GameBoard.CornerBL.y <= position.y &&
            position.y <= GameState.GameBoard.CornerTR.y)
            board = GameState.GameBoard;
        else if (GameState.SideBoard.CornerBL.x <= position.x &&
            position.x <= GameState.SideBoard.CornerTR.x &&
            GameState.SideBoard.CornerBL.y <= position.y &&
            position.y <= GameState.SideBoard.CornerTR.y)
            board = GameState.SideBoard;
        
        // If it is inside a board, get the space/piece
        Vector2Int space = new(-1, -1);
        if (board != null) space = board.GetSpace(position);

        // Pick up a piece when the screen is clicked/pressed
        if (Input.GetMouseButtonDown(0) && board != null)
        {
            HeldPiece = board.Spaces[space.x, space.y];
            if (HeldPiece != null)
                if (HeldPiece.IsPlayerPiece)
                {
                    board.RemovePiece(HeldPiece);
                    HeldPiece.transform.position = position - (Vector3.forward * 10);
                }
                else HeldPiece = null;
        }
        
        // Drag the selected piece as the pointer moves
        else if (Input.GetMouseButton(0) && HeldPiece != null)
        {
            HeldPiece.transform.position = position - (Vector3.forward * 10);
        }
        
        // Drop the piece in a new space (Or the old one)
        else if (Input.GetMouseButtonUp(0) && HeldPiece != null)
        {
            if (board != null && board.OnBoard(space) && !board.HasPiece(space)) board.AddPiece(HeldPiece, space);
            else HeldPiece.Board.AddPiece(HeldPiece, HeldPiece.Space);
            HeldPiece = null;
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
