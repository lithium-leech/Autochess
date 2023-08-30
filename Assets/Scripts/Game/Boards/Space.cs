using UnityEngine;

/// <summary>
/// A single space on the game board
/// </summary>
public class Space
{
    /// <summary>The board that this space is a part of</summary>
    public Board Board { get; }

    /// <summary>This space's x position</summary>
    public int X { get; }

    /// <summary>This space's y position</summary>
    public int Y { get; }

    /// <summary>This space's coordinates</summary>
    public Vector2Int Coordinates { get { return new Vector2Int(X, Y); } }

    /// <summary>This space's world position (z is 0)</summary>
    public Vector3 Position { get { return Board.ToPosition(Coordinates); } }

    /// <summary>True if enemy pieces can be promoted on this space</summary>
    public bool IsEnemyPromotion { get; set; } = false;

    /// <summary>True if player pieces can be promoted on this space</summary>
    public bool IsPlayerPromotion { get; set; } = false;

    /// <summary>The piece on this space (null when there is none)</summary>
    private Piece Piece { get; set; }

    /// <summary>The terrain on this space (null when there is none)</summary>
    private Terrain Terrain { get; set; }

    /// <summary>The equipment on this space (null when there is none)</summary>
    private Equipment Equipment { get; set; }

    /// <summary>The power on this space (null when there is none)</summary>
    private Power Power { get; set;}

    /// <summary>Creates a new instance of a Space</summary>
    /// <param name="board">The board that this space is a part of</param>
    /// <param name="x">The x position of this space</param>
    /// <param name="y">The y position of this space</param>
    public Space(Board board, int x, int y)
    {
        Board = board;
        X = x;
        Y = y;
    }

    public void Exit(Piece piece)
    {
        // Exit any terrain
        if (Terrain != null) Terrain.OnExit(piece);

        // Remove the piece from the space
        RemoveObject(piece);
    }

    public void Enter(Piece piece)
    {
        // Capture the existing piece
        if (Piece != null) piece.Captured = Piece;

        // Enter any terrain
        if (Terrain != null) Terrain.OnEnter(piece);

        // Add the piece to the space
        AddToSpace(piece);
    }

    /// <summary>Picks up an object from this space</summary>
    /// <returns>The first grabable object, or null if there are none</returns>
    public ChessObject Grab()
    {
        // Grab any possible pieces
        if (Piece != null && Piece.IsGrabable)
        {
            Piece grabbedPiece = Piece;
            Piece = null;
            Board.PlayerPieces.Remove(grabbedPiece);
            return grabbedPiece;
        }

        // Grab any possible equipment
        if (Equipment != null && Equipment.IsGrabable)
        {
            Equipment grabbedEquipment = Equipment;
            Equipment = null;
            return grabbedEquipment;
        }

        // Grab any possible terrain
        if (Terrain != null && Terrain.IsGrabable)
        {
            Terrain grabbedTerrain = Terrain;
            Terrain = null;
            return grabbedTerrain;
        }

        // Nothing was grabable
        return null;
    }

    /// <summary>Adds an object to this space</summary>
    /// <param name="obj">The object to add</param>
    /// <returns>True if the object was added successfully</returns>
    public bool AddObject(ChessObject obj)
    {
        // Check if the object can be added
        if (!IsEnterable(obj)) return false;

        // Add the object to the space
        AddToSpace(obj);

        // The object was successfully added
        return true;
    }

    private void AddToSpace(ChessObject obj)
    {
        // Update the object
        obj.Board = Board;
        obj.Space = this;

        // Add a piece
        if (obj is Piece piece)
        {
            if (piece.IsPlayer) Board.PlayerPieces.Add(piece);
            else Board.EnemyPieces.Add(piece);
            Piece = piece;
            piece.WarpTo(Board.ToPosition(Coordinates) + GameState.StillPieceZ);
        }

        // Add an equipment
        if (obj is Equipment equipment)
        {
            if (Piece == null)
            {
                Equipment = equipment;
                equipment.WarpTo(Board.ToPosition(Coordinates) + GameState.StillPieceZ);   
            }
            else if (Piece.Equipment == null) equipment.Equip(Piece);
            else throw new System.Exception("Equipment was placed on an invalid space");
        }

        // Add a terrain
        if (obj is Terrain terrain)
        {
            Terrain = terrain;
            terrain.WarpTo(Board.ToPosition(Coordinates) + GameState.TerrainZ);
        }

        // Add a power
        if (obj is Power power)
        {
            Power = power;
            power.WarpTo(Board.ToPosition(Coordinates) + GameState.StillPieceZ);
        }
    }

    /// <summary>Removes an object from this space</summary>
    /// <param name="obj">The object to remove</param>
    /// <returns>True if the object was removed successfully</returns>
    public bool RemoveObject(ChessObject obj)
    {
        // Remove a piece
        if (obj is Piece piece)
        {
            if (piece != Piece) return false;
            if (piece.IsPlayer) Board.PlayerPieces.Remove(piece);
            else Board.EnemyPieces.Remove(piece);
            Piece = null;
            return true;
        }

        // Remove an equipment
        if (obj is Equipment equipment)
        {
            if (Equipment != equipment) return false;
            Equipment = null;
            return true;
        }

        // Remove a terrain
        if (obj is Terrain terrain)
        {
            if (Terrain != terrain) return false;
            Terrain = null;
            return true;
        }

        // Remove a power
        if (obj is Power power)
        {
            if (Power != power) return false;
            Power = null;
            return true;
        }

        // Failed to remove anything
        return false;
    }

    /// <summary>Destroys all objects on this space</summary>
    public void Clear()
    {
        // Clear pieces
        if (Piece != null) Piece.Destroy();
        Piece = null;

        // Clear equipment
        if (Equipment != null) Equipment.Destroy();
        Equipment = null;

        // Clear terrain
        if (Terrain != null) Terrain.Destroy();
        Terrain = null;

        // Clear powers
        if (Power != null) Power.Destroy();
        Power = null;
    }

    /// <summary>Checks if this space is empty</summary>
    /// <returns>True if this space is empty</returns>
    public bool IsEmpty() => Piece == null && Equipment == null && Terrain == null && Power == null;

    /// <summary>Checks if this space is enterable</summary>
    /// <returns>True if this space is enterable</returns>
    public bool IsEnterable(ChessObject obj)
    {
        if (Piece != null && obj is Equipment && Piece.Equipment == null) return true;
        if (Piece != null || Equipment != null || Power != null) return false;
        if (Terrain != null && obj is Terrain) return false;
        if (Terrain != null && !Terrain.IsEnterable(obj)) return false;
        return true;
    }

    /// <summary>Checks if this space has a piece on it</summary>
    /// <returns>True if this space has a piece on it</returns>
    public bool HasPiece()
    {
        if (Piece == null) return false;
        else return true;
    }

    /// <summary>Checks if this space has an ally piece on it</summary>
    /// <param name="player">True if player pieces are allies</param>
    /// <returns>True if this space has an ally piece on it</returns>
    public bool HasAlly(bool player)
    {
        if (Piece == null) return false;
        else if (Piece.IsPlayer == player) return true;
        else return false;
    }

    /// <summary>Checks if this space has an enemy piece on it</summary>
    /// <param name="player">True if non-player pieces are enemies</param>
    /// <returns>True if this space has an enemy piece on it</returns>
    public bool HasEnemy(bool player)
    {
        if (Piece == null) return false;
        else if (Piece.IsPlayer == player) return false;
        else return true;
    }

    /// <summary>Checks if this space has a capturable piece</summary>
    /// <param name="player">True if non-player pieces are enemies</param>
    /// <returns>True if this space has a capturable piece</returns>
    public bool HasCapturable(Piece piece)
    {
        if (Piece == null) return false;
        else return Piece.IsCapturable(piece);
    }

    /// <summary>Checks if this space is in the player zone</summary>
    /// <returns>True if this space is in the player zone</returns>
    public bool InPlayerZone() => Y < Board.PlayerRows;

    /// <summary>Checks if this space is in the enemy zone</summary>
    /// <returns>True if this space is in the enemy zone</returns>
    public bool InEnemyZone() => Board.Height - Y < Board.EnemyRows;

    /// <summary>Checks if this space is in the neutral zone</summary>
    /// <returns>True if this space is in the neutral zone</returns>
    public bool InNeutralZone() => Y >= Board.PlayerRows && Board.Height - Y >= Board.EnemyRows;

    /// <summary>Gets a space in a position relative to this one</summary>
    /// <param name="x">The relative horizontal distance</param>
    /// <param name="y">The relative vertical distance</param>
    /// <returns>A space relative to this one on the same board</returns>
    public Space GetRelativeSpace(int x, int y) => Board.GetSpace(Coordinates + new Vector2Int(x, y));

    /// <summary>Returns the log display message for this space</summary>
    /// <returns>A string with notable information, or an empty string if there is nothing notable</returns>
    public string LogDisplay()
    {
        string str = "";
        if (Piece != null)
        {
            string controller = Piece.IsPlayer ? "Player" : "Enemy";
            string equipment = "";
            if (Piece.Equipment != null) equipment = $"({Piece.Equipment.Kind})";
            str += $" {controller}{Piece.Kind}{equipment}[{X},{Y}]";
        }
        if (Terrain != null)
        {
            string controller = Terrain.IsPlayer ? "Player" : "Enemy";
            str += $" {controller}{Terrain.Kind}[{X},{Y}]";
        }
        return str;
    }
}
