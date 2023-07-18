using System.Collections.Generic;
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

    /// <summary>The piece on this space (null when there is none)</summary>
    private Piece Piece { get; set; }

    /// <summary>The terrain affecting this space</summary>
    private IList<Terrain> Terrain { get; } = new List<Terrain>();

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
        // Exit terrain
        foreach (Terrain terrain in Terrain) terrain.OnExit(piece);

        // Remove the piece from the space
        RemoveObject(piece);
    }

    public void Enter(Piece piece)
    {
        // Do nothing if this piece is already on this space
        if (Piece == piece) return;

        // Capture the existing pieces
        if (Piece != null) piece.Captured = Piece;

        // Enter terrain
        foreach (Terrain terrain in Terrain) terrain.OnEnter(piece);

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

        // Grab any possible terrain
        Terrain grabbedTerrain = null;
        foreach (Terrain terrain in Terrain)
        {
            if (terrain.IsGrabable)
            {
                grabbedTerrain = terrain;
                break;
            }
        }
        if (grabbedTerrain != null)
        {
            Terrain.Remove(grabbedTerrain);
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

        // Warp the piece to its new location

        // The object was successfully added
        return true;
    }

    private void AddToSpace(ChessObject obj)
    {
        // Update the object
        obj.Board = Board;
        obj.Space = this;

        // Add a piece
        if (obj is Piece)
        {
            Piece piece = (Piece)obj;
            if (piece.IsPlayer) Board.PlayerPieces.Add(piece);
            else Board.EnemyPieces.Add(piece);
            Piece = piece;
            piece.WarpTo(Board.ToPosition(Coordinates) + GameState.PieceZBottom);
        }

        // Add a terrain
        if (obj is Terrain)
        {
            Terrain terrain = (Terrain)obj;
            Terrain.Add(terrain);
            terrain.WarpTo(Board.ToPosition(Coordinates) + GameState.TerrainZ);
        }
    }

    /// <summary>Removes an object from this space</summary>
    /// <param name="obj">The object to remove</param>
    /// <returns>True if the object was removed successfully</returns>
    public bool RemoveObject(ChessObject obj)
    {
        // Remove a piece
        if (obj is Piece)
        {
            Piece piece = (Piece)obj;
            if (piece != Piece) return false;
            if (piece.IsPlayer) Board.PlayerPieces.Remove(piece);
            else Board.EnemyPieces.Remove(piece);
            Piece = null;
            return true;
        }

        // Remove a terrain
        if (obj is Terrain)
        {
            Terrain terrain = (Terrain)obj;
            if (!Terrain.Contains(terrain)) return false;
            Terrain.Remove(terrain);
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

        // Clear terrain
        IList<Terrain> terrainToClear = new List<Terrain>(Terrain);
        foreach (Terrain terrain in terrainToClear) terrain.Destroy();
        Terrain.Clear();
    }

    /// <summary>Checks if this space is empty</summary>
    /// <returns>True if this space is empty</returns>
    public bool IsEmpty() => Piece == null && Terrain.Count < 1;

    /// <summary>Checks if this space is enterable</summary>
    /// <returns>True if this space is enterable</returns>
    public bool IsEnterable(ChessObject obj)
    {
        if (Piece != null) return false;
        foreach (Terrain terrain in Terrain)
            if (!terrain.IsEnterable(obj)) return false;
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
        if (Piece != null)
        {
            string controller = Piece.IsPlayer ? "Player" : "Enemy";
            return $" {controller}{Piece.Kind}[{X},{Y}]";
        }
        else return null;
    }
}
