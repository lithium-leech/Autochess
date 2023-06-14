using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A single game piece
/// </summary>
public abstract class Piece : MonoBehaviour
{
    /// <summary>The board this piece is on</summary>
    public Board Board { get; set; }

    /// <summary>The piece's coordinates on the board</summary>
    public Vector2Int Space { get; set; }
    
    /// <summary>True if this piece is controlled by the player</summary>
    public bool IsPlayer { get; set; }
    
    /// <summary>True if the piece is white</summary>
    public bool IsWhite { get; set; }
    
    /// <summary>True if this piece has been captured</summary>
    public bool IsCaptured { get; set; } = false;

    /// <summary>The piece to transform into</summary>
    public AssetGroup.Piece Transform { get; set; } = AssetGroup.Piece.None;

    /// <summary>True if the piece is moving towards a target location</summary>
    private bool IsMoving { get; set; } = false;
    
    /// <summary>The target location this piece is moving towards</summary>
    private Vector3 Target { get; set; }

    /// <summary>The incrementor used for lerp movement</summary>
    private float LerpIncrement { get; set; } = 0;

    /// <summary>The kind of piece this is</summary>
    public abstract AssetGroup.Piece Kind { get; }

    /// <summary>Takes the piece's turn</summary>
    public abstract void TakeTurn();

    void Update()
    {
        // Move towards the target when moving is activated
        if (IsMoving)
        {
            LerpIncrement += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, Target, LerpIncrement);
            if (Vector3.Distance(transform.position, Target) == 0) IsMoving = false;
        }
        else
        {
            // Change to a different piece when initiated
            if (Transform != AssetGroup.Piece.None)
            {
                Board.CapturePiece(this);
                Board.AddPiece(Transform, IsPlayer, IsWhite, Space);
            }
        }

        // Destroy the object when captured
        if (IsCaptured) GameObject.Destroy(gameObject);
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
            if (Board.HasPiece(pointer)) break;
            else possibleMoves.Add(pointer);
        }
    }

    /// <summary>Moves a piece from its current space to the given space</summary>
    /// <param name="space">The space to move to</param>
    protected void EnactTurn(Vector2Int space)
    {
        // Check that this piece is where it's supposed to be
        if (Board.Spaces[Space.x, Space.y] != this)
        {
            // Destroy this piece if it is not
            Board.CapturePiece(this);
            return;
        }

        // Do nothing if remaining stationary
        if (Space == space) return;

        // Destroy the captured piece if there is one
        Piece captured = Board.Spaces[space.x, space.y];
        if (captured != null) Board.CapturePiece(captured);

        // Update the location
        Board.Spaces[Space.x, Space.y] = null;
        Board.Spaces[space.x, space.y] = this;
        Space = space;

        // Move to the new location
        MoveTo(space);
    }

    /// <summary>Warps the piece to the specified location</summary>
    /// <param name="space">The space to warp to</param>
    public void WarpTo(Vector2Int space)
    {
        Vector3 position = Board.ToPosition(space);
        transform.position = position;
        Target = position;
        IsMoving = true;
        LerpIncrement = 0;
    }

    /// <summary>Warps the piece to the specified location</summary>
    /// <param name="space">The world coordinates to warp to</param>
    public void WarpTo(Vector3 position)
    {
        transform.position = position;
        Target = position;
        IsMoving = true;
        LerpIncrement = 0;
    }

    /// <summary>Moves the piece to the specified location</summary>
    /// <param name="space">The space to move to</param>
    public void MoveTo(Vector2Int space)
    {
        Vector3 position = Board.ToPosition(space);
        Target = position;
        IsMoving = true;
        LerpIncrement = 0;
    }
}
