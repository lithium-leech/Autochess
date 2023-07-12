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
            Space.RemoveObject(this);
            Board.AddPiece(Transform, IsPlayer, IsWhite, new Vector2Int(Space.X, Space.Y));
            Destroy();
        }
    }

    public override void Destroy()
    {
        if (Board != null)
        {
            Board.Spaces[Space.X, Space.Y].RemoveObject(this);
            if (IsPlayer) Board.PlayerPieces.Remove(this);
            else Board.EnemyPieces.Remove(this);
        }
        GameObject.Destroy(gameObject);
    }

    /// <summary>Searches for possible choices using the given direction increments</summary>
    /// <param name="xi">x increment</param>
    /// <param name="yi">y increment</param>
    /// <param name="range">The range a piece can move</param>
    /// <param name="possibleMoves">A list of possible moves to add to</param>
    /// <param name="possibleCaptures">A list of possible captures to add to</param>
    protected void GetChoicesInDirection(int xi, int yi, int range, IList<Space> possibleMoves, IList<Space> possibleCaptures)
    {
        Vector2Int pointer = new Vector2Int(Space.X, Space.Y);
        for (int i = 0; i < range; i++)
        {
            pointer.x += xi;
            pointer.y += yi;
            if (!Board.OnBoard(pointer)) break;
            Space space = Board.Spaces[pointer.x, pointer.y];
            if (space.HasEnemy(IsPlayer)) { possibleCaptures.Add(space); break; }
            if (!space.IsPassable()) break;
            possibleMoves.Add(space);
        }
    }
}
