using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A single game piece
/// </summary>
public abstract class Piece
{
    /// <summary>The piece's in-game sprite</summary>
    public PieceBehavior Behavior { get; }

    /// <summary>True if this piece is controlled by the player</summary>
    public bool IsPlayerPiece { get; }

    /// <summary>The board this piece is on</summary>
    public Board Board { get; set; }

    /// <summary>The piece's coordinates on the board</summary>
    public Vector2Int Space { get; set; }

    /// <summary>Provides random number generation</summary>
    protected System.Random RND { get; } = new();

    /// <summary>Base constructor for Piece abstractions</summary>
    /// <param name="unityObject">The unity object behind this piece</param>
    /// <param name="isPlayerPiece">True if this Piece is controlled by the player</param>
    public Piece(GameObject unityObject, bool isPlayerPiece)
    {
        Behavior = unityObject.GetComponent<PieceBehavior>();
        IsPlayerPiece = isPlayerPiece;
    }

    /// <summary>Enacts this piece's move for a single turn</summary>
    public abstract void Move();

    /// <summary>Destroys this piece</summary>
    public void Capture()
    {
        if (Board.Spaces[Space.x,Space.y] == this) Board.Spaces[Space.x, Space.y] = null;
        if (IsPlayerPiece) Board.PlayerPieces.Remove(this);
        else Board.EnemyPieces.Remove(this);
        Behavior.Captured();
    }

    /// <summary>Checks if a space has a piece on it</summary>
    /// <param name="space">The space to check</param>
    /// <returns>True if the space has a piece on it</returns>
    public bool HasPiece(Vector2Int space)
    {
        if (OnBoard(space))
        {
            if (Board.Spaces[space.x, space.y] != null)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>Checks if a space has an enemy piece on it</summary>
    /// <param name="space">The space to check</param>
    /// <returns>True if the space has an enemy piece on it</returns>
    public bool HasEnemy(Vector2Int space)
    {
        if (!HasPiece(space)) return false;
        else if (Board.Spaces[space.x, space.y].IsPlayerPiece == IsPlayerPiece) return false;
        else return true;
    }

    /// <summary>Checks if a space is on the board</summary>
    /// <param name="space">The space to check</param>
    /// <returns>True if the space is on the board</returns>
    public bool OnBoard(Vector2Int space)
    {
        return space.x >= 0 && space.x < Board.Width && space.y >= 0 && space.y < Board.Height;
    }

    /// <summary>Searches for possible choices using the given direction increments</summary>
    /// <param name="xi">x increment</param>
    /// <param name="yi">y increment</param>
    /// <param name="possibleMoves">A list of possible moves to add to</param>
    /// <param name="possibleCaptures">A list of possible captures to add to</param>
    public void GetChoicesInDirection(int xi, int yi, IList<Vector2Int> possibleMoves, IList<Vector2Int> possibleCaptures)
    {
        Vector2Int pointer = new(Space.x, Space.y);
        int maxLoops = Mathf.Max(Board.Width, Board.Height);
        for (int i = 0; i < maxLoops; i++)
        {
            pointer.x += xi;
            pointer.y += yi;
            if (!OnBoard(pointer)) break;
            if (HasEnemy(pointer)) { possibleCaptures.Add(pointer); break; }
            if (!HasPiece(pointer)) possibleMoves.Add(pointer);
        }
    }
}
