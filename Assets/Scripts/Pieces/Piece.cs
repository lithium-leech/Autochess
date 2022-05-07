using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A single game piece
/// </summary>
public abstract class Piece : MonoBehaviour
{
    /// <summary>True if this piece is controlled by the player</summary>
    public bool IsPlayerPiece { get; set; }

    /// <summary>The board this piece is on</summary>
    public Board Board { get; set; }

    /// <summary>The piece's coordinates on the board</summary>
    public Vector2Int Space { get; set; }
    
    /// <summary>True if this piece has been captured</summary>
    private bool IsCaptured { get; set; } = false;

    /// <summary>True if the piece is moving towards a target location</summary>
    private bool IsMoving { get; set; } = false;
    
    /// <summary>The target location this piece is moving towards</summary>
    private Vector3 Target { get; set; }

    /// <summary>The z-coordinate that pieces exist at</summary>
    private float Z { get; } = -1.0f;

    /// <summary>The incrementor used for lerp movement</summary>
    private float LerpI { get; set; } = 0;

    /// <summary>Enacts this piece's move for a single turn</summary>
    public abstract void TakeTurn();

    void Update()
    {
        // Move towards the target when moving is activated
        if (IsMoving)
        {
            LerpI += Time.deltaTime/100;
            transform.position = Vector3.Lerp(transform.position, Target, LerpI);
            if (Vector3.Distance(transform.position, Target) == 0) IsMoving = false;
        }

        // Destroy the object when captured
        if (IsCaptured) GameObject.Destroy(gameObject);
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

    /// <summary>Warps the piece to the specified location</summary>
    /// <param name="position">The world coordinates to warp to</param>
    public void WarpTo(Vector2 position)
    {

        transform.position = AddZ(position);
        Target = AddZ(position);
        IsMoving = true;
        LerpI = 0;
    }

    /// <summary>Moves the piece to the specified location</summary>
    /// <param name="position">The world coordinates to move to</param>
    public void MoveTo(Vector2 position)
    {
        Target = AddZ(position);
        IsMoving = true;
        LerpI = 0;
    }

    /// <summary>Destroys this piece</summary>
    public void Capture()
    {
        if (Board.Spaces[Space.x, Space.y] == this) Board.Spaces[Space.x, Space.y] = null;
        if (IsPlayerPiece) Board.PlayerPieces.Remove(this);
        else Board.EnemyPieces.Remove(this);
        IsCaptured = true;
    }

    /// <summary>Adds a z-coordinate to the given position</summary>
    /// <param name="position">The position to add a z-coordinate to</param>
    /// <returns>A Vector3</returns>
    private Vector3 AddZ(Vector2 position) => new(position.x, position.y, Z);
}
