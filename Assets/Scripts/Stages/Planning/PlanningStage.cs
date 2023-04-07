using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The stage of the game where the player chooses where to put their pieces
/// </summary>
public class PlanningStage : IStage
{
    /// <summary>Creates a new instance of a PlanningStage</summary>
    /// <param name="game">The Game to run the PlanningStage in</param>
    public PlanningStage(Game game) => Game = game;

    /// <summary>The Game this is being run in</summary>
    private Game Game { get; set; }

    /// <summary>The Piece currently being moved around by the player</summary>
    public Piece HeldPiece { get; set; }

    /// <summary>Space highlights displayed during the planning phase</summary>
    public IEnumerable<GameObject> Highlights { get; set; }

    public void Start()
    {
        // Start the planning music
        GameState.MusicBox.PlayMusic(SongName.Planning);

        // Set up the boards
        PlacePieces();

        // Add highlights around spaces that the player can put pieces
        List<GameObject> highlights = new();
        highlights.AddRange(AddHighlights(Game.GameBoard));
        highlights.AddRange(AddHighlights(Game.SideBoard));
        highlights.AddRange(AddHighlights(Game.TrashBoard));
        Highlights = highlights;

        // Activate the fight button
        Game.FightButton.onClick.AddListener(StartFight);
        Game.FightButton.interactable = true;
    }

    public void During()
    {
        // Get the current mouse position
        Vector3 position = GameState.Camera.ScreenToWorldPoint(Input.mousePosition);
        position.z = GameState.PieceZ;

        // Check if the position is inside a board
        Board board = null;
        if (Game.GameBoard.CornerBL.x <= position.x &&
            position.x <= Game.GameBoard.CornerTR.x &&
            Game.GameBoard.CornerBL.y <= position.y &&
            position.y <= Game.GameBoard.CornerTR.y)
            board = Game.GameBoard;
        else if (Game.SideBoard.CornerBL.x <= position.x &&
            position.x <= Game.SideBoard.CornerTR.x &&
            Game.SideBoard.CornerBL.y <= position.y &&
            position.y <= Game.SideBoard.CornerTR.y)
            board = Game.SideBoard;
        else if (Game.TrashBoard.CornerBL.x <= position.x &&
            position.x <= Game.TrashBoard.CornerTR.x &&
            Game.TrashBoard.CornerBL.y <= position.y &&
            position.y <= Game.TrashBoard.CornerTR.y)
            board = Game.TrashBoard;

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

    public void End()
    {
        // De-activate the fight button
        Game.FightButton.onClick.RemoveAllListeners();
        Game.FightButton.interactable = false;

        // Remove highlights loaded in planning start
        foreach (GameObject highlight in Highlights) UnityEngine.Object.Destroy(highlight);
        Highlights = new List<GameObject>();

        // Save the current positions of the player's pieces
        Game.PlayerGameBoard.Clear();
        Game.PlayerSideBoard.Clear();
        foreach (Piece piece in Game.GameBoard.PlayerPieces)
            Game.PlayerGameBoard.Add(new PositionRecord(piece.Kind, piece.Space));
        foreach (Piece piece in Game.SideBoard.PlayerPieces)
            Game.PlayerSideBoard.Add(new PositionRecord(piece.Kind, piece.Space));
    }

    /// <summary>Starts the fight sequence</summary>
    public void StartFight()
    {
        // Don't start the fight if there are no pieces on the board
        if (Game.GameBoard.PlayerPieces.Count < 1) return;

        // De-activate the fight button
        Game.FightButton.onClick.RemoveAllListeners();
        Game.FightButton.interactable = false;

        // Queue the combat stage
        Game.NextStage = new CombatStage(Game);
    }

    /// <summary>Sets up the peices on the game board and side board</summary>
    public void PlacePieces()
    {
        // Clear both boards
        Game.GameBoard.Clear();
        Game.SideBoard.Clear();

        // Place the enemies peices on the game board
        foreach (AssetGroup.Piece pieceType in Game.EnemyPieces)
        {
            Vector2Int space = GetRandomEmptySpace();
            Game.GameBoard.AddPiece(pieceType, true, space);
        }

        // Place the player's peices on the game board
        foreach (PositionRecord record in Game.PlayerGameBoard)
        {
            if (record.Space == null)
            {
                if (Game.PlayerSideBoard.Count < 16) Game.PlayerSideBoard.Add(record);
            }
            else
            {
                Game.GameBoard.AddPiece(record.Kind, false, record.Space.Value);
            }
        }

        // Place the player's peices on the side board
        foreach (PositionRecord record in Game.PlayerSideBoard)
        {
            if (record.Space == null)
                Game.SideBoard.AddPiece(record.Kind, false);
            else
                Game.SideBoard.AddPiece(record.Kind, false, record.Space.Value);
        }
    }

    /// <summary>Add highlights around the player rows of the given board</summary>
    /// <param name="board">The board to add highlights to</param>
    /// <returns>The created highlight objects</returns>
    public IEnumerable<GameObject> AddHighlights(Board board)
    {
        // Start with an empty list
        IList<GameObject> highlights = new List<GameObject>();

        // No highlights needed if there are 0 player rows
        if (board.PlayerRows > 0)
        {
            // Create the bottom row
            highlights.Add(Game.CreateHighlight(AssetGroup.Highlight.BottomLeft, board, new Vector2Int(-1, -1)));
            for (int i = 0; i < board.Width; i++) highlights.Add(Game.CreateHighlight(AssetGroup.Highlight.Bottom, board, new Vector2Int(i, -1)));
            highlights.Add(Game.CreateHighlight(AssetGroup.Highlight.BottomRight, board, new Vector2Int(board.Width, -1)));

            // Create the left and right columns
            for (int i = 0; i < board.PlayerRows; i++)
            {
                highlights.Add(Game.CreateHighlight(AssetGroup.Highlight.Left, board, new Vector2Int(-1, i)));
                highlights.Add(Game.CreateHighlight(AssetGroup.Highlight.Right, board, new Vector2Int(board.Width, i)));
            }

            // Create the top row
            highlights.Add(Game.CreateHighlight(AssetGroup.Highlight.TopLeft, board, new Vector2Int(-1, board.PlayerRows)));
            for (int i = 0; i < board.Width; i++) highlights.Add(Game.CreateHighlight(AssetGroup.Highlight.Top, board, new Vector2Int(i, board.PlayerRows)));
            highlights.Add(Game.CreateHighlight(AssetGroup.Highlight.TopRight, board, new Vector2Int(board.Width, board.PlayerRows)));
        }

        // Return the generated highlights
        return highlights;
    }

    /// <summary>Gets a random empty space on the enemy's side of the game board</summary>
    /// <returns>An empty space on the game board</returns>
    public Vector2Int GetRandomEmptySpace()
    {
        IList<Vector2Int> emptySpaces = new List<Vector2Int>();
        for (int x = 0; x < Game.GameBoard.Width; x++)
            for (int y = 6; y < Game.GameBoard.Height; y++)
            {
                Vector2Int space = new(x, y);
                if (!Game.GameBoard.HasPiece(space)) emptySpaces.Add(space);
            }
        return emptySpaces[UnityEngine.Random.Range(0, emptySpaces.Count - 1)];
    }
}
