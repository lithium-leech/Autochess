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

    /// <summary>The object currently being held by the player</summary>
    public ChessObject HeldObject { get; set; }

    /// <summary>Space highlights displayed during the planning phase</summary>
    public IEnumerable<GameObject> Highlights { get; set; }

    public void Start()
    {
        // Start the planning music
        GameState.MusicBox.PlayMusic(SongName.Planning);

        // Set up the boards
        Game.GameBoard.Clear();
        Game.SideBoard.Clear();
        SetUpPlayer();
        PlacementAI.SetUpEnemy(Game);

        // Add highlights around spaces that the player can put pieces
        List<GameObject> highlights = new();
        highlights.AddRange(AddHighlights(Game.GameBoard));
        highlights.AddRange(AddHighlights(Game.SideBoard));
        Highlights = highlights;
        
        // Activate the concede button
        EndConcede();
        Game.ConcedeButton.SetEnabled(true);
        Game.ConcedeButton.onClick.AddListener(StartConcede);

        // Activate the fight button
        Game.FightButton.onClick.AddListener(StartFight);
        if (Game.GameBoard.PlayerPieces.Count > 0) Game.FightButton.SetEnabled(true);
    }

    public void During()
    {
        // Get the current mouse position
        Vector3 position = GameState.Camera.ScreenToWorldPoint(Input.mousePosition);
        position.z = GameState.MovingPieceZ.z;

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

        // If it is inside a board, get the space
        Space space = null;
        if (board != null) space = board.GetSpace(board.ToSpace(position));

        // Pick up an object when the screen is clicked/pressed
        if (Input.GetMouseButtonDown(0) && board != null && space != null && HeldObject == null)
        {
            HeldObject = space.Grab();
            if (HeldObject != null)
                HeldObject.transform.position = position - (Vector3.forward * 10);
        }

        // Drag the selected object as the pointer moves
        else if (Input.GetMouseButton(0) && HeldObject != null)
        {
            HeldObject.transform.position = position - (Vector3.forward * 10);
        }

        // Drop the object in a new space (Or the old one)
        else if (Input.GetMouseButtonUp(0) && HeldObject != null)
        {
            bool placed = false;
            if (board != null && space != null && HeldObject.IsPlaceable(space))
                placed = space.AddObject(HeldObject);
            if (!placed)
                HeldObject.Space.AddObject(HeldObject);
            HeldObject = null;

            // De-activate the fight button when there are no pieces on the board
            if (Game.GameBoard.PlayerPieces.Count > 0) Game.FightButton.SetEnabled(true);
            else Game.FightButton.SetEnabled(false);
        }
    }

    public void End()
    {
        // De-activate the fight button
        Game.FightButton.onClick.RemoveAllListeners();
        Game.FightButton.SetEnabled(false);

        // De-activate the concede button
        Game.ConcedeButton.onClick.RemoveAllListeners();
        Game.ConcedeButton.SetEnabled(false);
        EndConcede();

        // Remove highlights loaded in planning start
        foreach (GameObject highlight in Highlights) UnityEngine.Object.Destroy(highlight);
        Highlights = new List<GameObject>();

        // Save the current positions of the player's pieces
        Game.PlayerGameBoard.Clear();
        Game.PlayerSideBoard.Clear();
        foreach (Piece piece in Game.GameBoard.PlayerPieces)
            Game.PlayerGameBoard.Add(new PiecePositionRecord(piece.Kind, piece.Space));
        foreach (Piece piece in Game.SideBoard.PlayerPieces)
            Game.PlayerSideBoard.Add(new PiecePositionRecord(piece.Kind, piece.Space));
    }

    /// <summary>Starts the fight sequence</summary>
    public void StartFight()
    {
        // Don't start the fight if there are no pieces on the board
        if (Game.GameBoard.PlayerPieces.Count < 1) return;

        // De-activate the fight button
        Game.FightButton.onClick.RemoveAllListeners();
        Game.FightButton.SetEnabled(false);

        // Queue the combat stage
        Game.NextStage = new CombatStage(Game);
    }

    /// <summary>Displays the concede menu to the player</summary>
    private void StartConcede()
    {
        MenuManager.AddActiveMenu(Game.ConcedeMenu);
        Game.CancelConcedeButton.onClick.AddListener(EndConcede);
        Game.ConfirmConcedeButton.onClick.AddListener(ConfirmConcede);
    }

    /// <summary>The player concedes and the fight is lost</summary>
    private void ConfirmConcede()
    {
        Game.NextStage = new DefeatStage(Game);
        EndConcede();
    }

    /// <summary>Removes the concede menu and resets concede logic</summary>
    private void EndConcede()
    {
        MenuManager.RemoveActiveMenu(Game.ConcedeMenu);
        Game.CancelConcedeButton.onClick.RemoveAllListeners();
        Game.ConfirmConcedeButton.onClick.RemoveAllListeners();
    }

    /// <summary>Sets up the player's pieces on the game board and side board</summary>
    private void SetUpPlayer()
    {
        // Place the player's pieces on the game board
        foreach (PiecePositionRecord record in Game.PlayerGameBoard)
        {
            if (record.Space == null || !record.Space.InPlayerZone())
            {
                record.Space = null;
                if (Game.PlayerSideBoard.Count < 24) Game.PlayerSideBoard.Add(record);
            }
            else
            {
                Game.GameBoard.AddPiece(record.Kind, true, GameState.IsPlayerWhite, record.Space);
            }
        }

        // Place the player's pieces on the side board
        foreach (PiecePositionRecord record in Game.PlayerSideBoard)
        {
            if (record.Space == null || !record.Space.IsEmpty())
                Game.SideBoard.AddPiece(record.Kind, true, GameState.IsPlayerWhite);
            else
                Game.SideBoard.AddPiece(record.Kind, true, GameState.IsPlayerWhite, record.Space);
        }

        // Place the player's objects on the side board
        foreach (AssetGroup.Object objectType in Game.PlayerObjects)
            Game.SideBoard.AddObject(objectType, true, GameState.IsPlayerWhite);
    }

    /// <summary>Add highlights around the player rows of the given board</summary>
    /// <param name="board">The board to add highlights to</param>
    /// <returns>The created highlight objects</returns>
    private IEnumerable<GameObject> AddHighlights(Board board)
    {
        // Start with an empty list
        IList<GameObject> highlights = new List<GameObject>();

        // Add player zone highlights
        if (board.PlayerRows > 0) AddHighlightZone(board, highlights, true);

        // Add enemy zone highlights
        if (board.EnemyRows > 0) AddHighlightZone(board, highlights, false);

        // Return the generated highlights
        return highlights;
    }

    /// <summary>Adds a single zone of highlights</summary>
    /// <param name="board">The board to add highlights to</param>
    /// <param name="highlights">The collection of highlights to add to</param>
    /// <param name="player">True if this is a player zone</param>
    private void AddHighlightZone(Board board, IList<GameObject> highlights, bool player)
    {
        // Get the zone coordinates
        int top = player ? board.PlayerRows - 1 : board.Height - 1;
        int bottom = player ? 0 : board.Height - board.EnemyRows;
        int right = board.Width - 1;
        int left = 0;
        Vector4 zone = new(bottom, left, top, right);

        // Create highlights for all coordinates in the zone
        for (int x = left - 1; x <= right + 1; x++)
        for (int y = bottom - 1; y <= top + 1; y++)
        {
            Vector2Int coordinates = new(x, y);
            Space space = GetZoneSpace(board, zone, coordinates);
            if (space == null)
            {
                AssetGroup.Highlight kind = DetermineHighlightKind(board, zone, coordinates);
                if (kind != AssetGroup.Highlight.None)
                    highlights.Add(Game.CreateHighlight(kind, player, board, coordinates));
            }
        }
    }

    /// <summary>Determines the kind of highlight to display at a given set of coordinates</summary>
    /// <param name="board">The board to display highlights on</param>
    /// <param name="zone">The zone to display highlights around</param>
    /// <param name="coordinates">The coordinates to determine highlighting for</param>
    /// <returns>The kind of highlight that belongs at the given coordinates</returns>
    private AssetGroup.Highlight DetermineHighlightKind(Board board, Vector4 zone, Vector2Int coordinates)
    {
        // Determine which spaces are present
        bool top = GetZoneSpace(board, zone, coordinates + new Vector2Int(0, 1)) != null;
        bool bottom = GetZoneSpace(board, zone, coordinates + new Vector2Int(0, -1)) != null;
        bool right = GetZoneSpace(board, zone, coordinates + new Vector2Int(1, 0)) != null;
        bool left = GetZoneSpace(board, zone, coordinates + new Vector2Int(-1, 0)) != null;
        bool topRight = GetZoneSpace(board, zone, coordinates + new Vector2Int(1, 1)) != null;
        bool topLeft = GetZoneSpace(board, zone, coordinates + new Vector2Int(-1, 1)) != null;
        bool bottomRight = GetZoneSpace(board, zone, coordinates + new Vector2Int(1, -1)) != null;
        bool bottomLeft = GetZoneSpace(board, zone, coordinates + new Vector2Int(-1, -1)) != null;

        // Create a key for the discovered information
        int sKey = (bottom ? 1 : 0) << 0 |
                   (top ? 1 : 0) << 1 |
                   (left ? 1 : 0) << 2 |
                   (right ? 1 : 0) << 3;
        int cKey = (bottomLeft ? 1 : 0) << 0 |
                   (bottomRight ? 1 : 0) << 1 |
                   (topLeft ? 1 : 0) << 2 |
                   (topRight ? 1 : 0) << 3;

        // Check the bordering sides
        AssetGroup.Highlight kind = sKey switch
        {
            1 => AssetGroup.Highlight.T,
            2 => AssetGroup.Highlight.B,
            3 => AssetGroup.Highlight.ITB,
            4 => AssetGroup.Highlight.R,
            5 => AssetGroup.Highlight.ITR,
            6 => AssetGroup.Highlight.IBR,
            7 => AssetGroup.Highlight.ITBR,
            8 => AssetGroup.Highlight.L,
            9 => AssetGroup.Highlight.ITL,
            10 => AssetGroup.Highlight.IBL,
            11 => AssetGroup.Highlight.ITBL,
            12 => AssetGroup.Highlight.IRL,
            13 => AssetGroup.Highlight.ITRL,
            14 => AssetGroup.Highlight.IBRL,
            15 => AssetGroup.Highlight.ITBRL,
            _ => AssetGroup.Highlight.None
        };
        if (kind != AssetGroup.Highlight.None) return kind;

        // Check the bordering corners
        return cKey switch
        {
            1 => AssetGroup.Highlight.TR,
            2 => AssetGroup.Highlight.TL,
            3 => AssetGroup.Highlight.TRTL,
            4 => AssetGroup.Highlight.BR,
            5 => AssetGroup.Highlight.TRBR,
            6 => AssetGroup.Highlight.TLBR,
            7 => AssetGroup.Highlight.TRTLBR,
            8 => AssetGroup.Highlight.BL,
            9 => AssetGroup.Highlight.TRBL,
            10 => AssetGroup.Highlight.TLBL,
            11 => AssetGroup.Highlight.TRTLBL,
            12 => AssetGroup.Highlight.BRBL,
            13 => AssetGroup.Highlight.TRBRBL,
            14 => AssetGroup.Highlight.TLBRBL,
            15 => AssetGroup.Highlight.TRTLBRBL,
            _ => AssetGroup.Highlight.None,
        };       
    }

    /// <summary>Gets a space inside of a given zone</summary>
    /// <param name="board">The board to get a space from</param>
    /// <param name="top">The zone to get a space within</param>
    /// <param name="coordinates">The coordinates to get a space for</param>
    /// <returns>A Space within the zone, or null</returns>
    private Space GetZoneSpace(Board board, Vector4 zone, Vector2Int coordinates)
    {
        if (coordinates.x < zone.y || coordinates.x > zone.w || coordinates.y < zone.x || coordinates.y > zone.z) return null;
        else return board.GetSpace(coordinates);
    }
}
