using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main game object, responsible for managing
/// the state of the game
/// </summary>
public class Game : MonoBehaviour
{
    /// Properties to set using Unity interface
    public GameObject[] PiecePrefabs;
    public GameObject[] HighlightPrefabs;
    public Camera Camera;

    /// <summary>The time elapsed so far, in between turns</summary>
    private float TimeWaited { get; set; }

    /// <summary>True if it is currently white's turn</summary>
    private bool WhiteTurn { get; set; }
    
    /// <summary>The Piece currently being moved around by the player</summary>
    private Piece HeldPiece { get; set; }

    /// <summary>Space highlights displayed during the planning phase</summary>
    private IEnumerable<GameObject> Highlights { get; set; }

    private void Start()
    {
        // Set game states
        GameState.PlanningStarted = false;
        GameState.InPlanning = false;
        GameState.PlanningOver = false;
        GameState.FightStarted = false;
        GameState.InFight = false;
        GameState.FightOver = false;
        GameState.Victory = false;

        // Create the game boards
        GameState.GameBoard = new Board(this, 8, 8, 2, new Vector2(-4.0f, 1.0f));
        GameState.SideBoard = new Board(this, 8, 3, 3, new Vector2(-4.0f, -4.0f));

        // Create a sample setup
        GameState.GameBoard.AddPiece<Pawn>(true, new Vector2Int(0, 7));
        GameState.GameBoard.AddPiece<Pawn>(true, new Vector2Int(2, 7));
        GameState.GameBoard.AddPiece<Pawn>(true, new Vector2Int(4, 6));
        GameState.GameBoard.AddPiece<Pawn>(true, new Vector2Int(7, 6));

        GameState.SideBoard.AddPiece<Pawn>(false);
        GameState.SideBoard.AddPiece<Pawn>(false);
        GameState.SideBoard.AddPiece<Pawn>(false);

        // Start the planning phase
        GameState.PlanningStarted = true;
    }

    private void Update()
    {
        if (!GameState.InPlanning && GameState.PlanningStarted) DetectPlanningStart();
        if (GameState.InPlanning) RunPlanning();
        if (GameState.InPlanning && GameState.PlanningOver) DetectPlanningFinish();
        if (!GameState.InFight && GameState.FightStarted) DetectFightStart();
        if (GameState.InFight) RunFight();
        if (GameState.InFight && GameState.FightOver) DetectFightFinish();
    }

    /// <summary>Starts the planning phase</summary>
    private void DetectPlanningStart()
    {
        // Only run the planning started sequence once
        GameState.PlanningStarted = false;
        GameState.InPlanning = true;

        // Add highlights around spaces that the player can put pieces
        List<GameObject> highlights = new();
        highlights.AddRange(AddHighlights(GameState.GameBoard));
        highlights.AddRange(AddHighlights(GameState.SideBoard));
        Highlights = highlights;
    }

    /// <summary>Handles player inputs during the planning phase</summary>
    private void RunPlanning()
    {
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
        if (board != null) space = board.ToSpace(position);

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
            if (board != null && board.IsPlayerSpace(space) && !board.HasPiece(space)) board.AddPiece(HeldPiece, space);
            else HeldPiece.Board.AddPiece(HeldPiece, HeldPiece.Space);
            HeldPiece = null;
        }
    }

    /// <summary>Performs end of planning phase operations</summary>
    private void DetectPlanningFinish()
    {
        // Only run the fight over sequence once
        GameState.PlanningOver = false;
        GameState.InPlanning = false;

        // Remove highlights loaded in planning start
        foreach (GameObject highlight in Highlights) Destroy(highlight);
        Highlights = new List<GameObject>();
    }

    /// <summary>Starts a fight once it's been switched on</summary>
    private void DetectFightStart()
    {
        // Only run the fight started sequence once
        GameState.FightStarted = false;
        GameState.InFight = true;

        // Set the initial fighting states
        TimeWaited = 0;
        WhiteTurn = true;
    }

    /// <summary>Runs the battle operations</summary>
    private void RunFight()
    {
        // Pause between turns
        TimeWaited += Time.deltaTime;
        if (TimeWaited < GameState.TurnPause) return;
         
        // Move the current player's pieces
        if (WhiteTurn)
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
        }
        else if (GameState.GameBoard.EnemyPieces.Count < 1)
        {
            GameState.Victory = true;
            GameState.FightOver = true;
        }

        // Go to the next turn
        WhiteTurn = !WhiteTurn;
        TimeWaited = 0;
    }

    /// <summary>Performs end of fight operations</summary>
    private void DetectFightFinish()
    {
        // Only run the fight over sequence once
        GameState.FightOver = false;
        GameState.InFight = false;
         
        // Determine if the battle was won
        if (GameState.Victory)
        {

        }
        else
        {

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

    /// <summary>Add highlights around the player rows of the given board</summary>
    /// <param name="board">The board to add highlights to</param>
    /// <returns>The created highlight objects</returns>
    private IEnumerable<GameObject> AddHighlights(Board board)
    {
        // Start with an empty list
        IList<GameObject> highlights = new List<GameObject>();

        // No highlights needed if there are 0 player rows
        if (board.PlayerRows > 0)
        {
            // Create the bottom row
            highlights.Add(CreateHighlight(0, board, new Vector2Int(-1, -1)));
            for (int i = 0; i < board.Width; i++) highlights.Add(CreateHighlight(7, board, new Vector2Int(i, -1)));
            highlights.Add(CreateHighlight(6, board, new Vector2Int(board.Width, -1)));

            // Create the left and right columns
            for (int i = 0; i < board.PlayerRows; i++)
            {
                highlights.Add(CreateHighlight(5, board, new Vector2Int(-1, i)));
                highlights.Add(CreateHighlight(1, board, new Vector2Int(board.Width, i)));
            }

            // Create the top row
            highlights.Add(CreateHighlight(2, board, new Vector2Int(-1, board.PlayerRows)));
            for (int i = 0; i < board.Width; i++) highlights.Add(CreateHighlight(3, board, new Vector2Int(i, board.PlayerRows)));
            highlights.Add(CreateHighlight(4, board, new Vector2Int(board.Width, board.PlayerRows)));
        }

        // Return the generated highlights
        return highlights;
    }

    /// <summary>Creates a single space highlight object</summary>
    /// <param name="prefab">The prefab index to load a highlight for</param>
    /// <param name="board">The board to load the highlight on</param>
    /// <param name="space">The space to place the highlight on</param>
    /// <returns>A new highlight object</returns>
    private GameObject CreateHighlight(int prefab, Board board, Vector2Int space)
    {
        GameObject highlight = Instantiate(HighlightPrefabs[prefab]);
        highlight.transform.position = board.ToPosition(space);
        return highlight;
    }
}
