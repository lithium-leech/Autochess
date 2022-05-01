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
    public void Captured()
    {
        if (Board.Spaces[Space.x,Space.y] == this) Board.Spaces[Space.x, Space.y] = null;
        if (IsPlayerPiece) Board.PlayerPieces.Remove(this);
        else Board.EnemyPieces.Remove(this);
        Behavior.Captured();
    }
}
