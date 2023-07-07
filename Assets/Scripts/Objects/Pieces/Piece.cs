using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A single game piece
/// </summary>
public abstract class Piece : ChessObject
{
    /// <summary>True if this piece is controlled by the player</summary>
    public bool IsPlayer { get; set; }
    
    /// <summary>True if the piece is white</summary>
    public bool IsWhite { get; set; }

    /// <summary>A piece to transform into</summary>
    public AssetGroup.Piece Transform { get; set; } = AssetGroup.Piece.None;

    /// <summary>The kind of piece this is</summary>
    public abstract AssetGroup.Piece Kind { get; }

    /// <summary>Takes the piece's turn</summary>
    public abstract void TakeTurn();

    protected override void Update2()
    {
        // Transform to a different piece when initiated
        if (!IsMoving && Transform != AssetGroup.Piece.None)
        {
            Board.Spaces[Space.x,Space.y].Remove(this);
            Board.AddPiece(Transform, IsPlayer, IsWhite, Space);
            Destroy();
        }
    }

    public override void Remove()
    {
        if (Board != null)
        {
            Board.Spaces[Space.x, Space.y].Remove(this);
            if (IsPlayer) Board.PlayerPieces.Remove(this);
            else Board.EnemyPieces.Remove(this);
            Board = null;
            Space = new Vector2Int(-1, -1);
        }
    }

    /// <summary>Searches for possible choices using the given direction increments</summary>
    /// <param name="xi">x increment</param>
    /// <param name="yi">y increment</param>
    /// <param name="range">The range a piece can move</param>
    /// <param name="possibleMoves">A list of possible moves to add to</param>
    /// <param name="possibleCaptures">A list of possible captures to add to</param>
    public void GetChoicesInDirection(int xi, int yi, int range, IList<Vector2Int> possibleMoves, IList<Vector2Int> possibleCaptures)
    {
        Vector2Int pointer = new Vector2Int(Space.x, Space.y);
        for (int i = 0; i < range; i++)
        {
            pointer.x += xi;
            pointer.y += yi;
            if (!Board.OnBoard(pointer)) break;
            if (Board.HasEnemy(IsPlayer, pointer)) { possibleCaptures.Add(pointer); break; }
            if (Board.GetPiece(pointer)) break;
            else possibleMoves.Add(pointer);
        }
    }

    /// <summary>Moves a piece from its current space to the given space</summary>
    /// <param name="space">The space to move to</param>
    protected void EnactTurn(Vector2Int space)
    {   
        // Check that this piece is where it's supposed to be
        if (Board.GetPiece(Space) != this)
        {
            // Destroy this piece if it is not
            Destroy();
            return;
        }

        // Do nothing if remaining stationary
        if (Space == space) return;

        // Destroy the captured piece if there is one
        Piece captured = Board.GetPiece(space);
        if (captured != null) captured.Destroy();

        // Update the location
        Board.Spaces[Space.x, Space.y].Clear();
        Board.Spaces[space.x, space.y].Add(this);
        Space = space;

        // Move to the new location
        MoveTo(space);
    }
}
