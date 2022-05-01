using UnityEngine;

/// <summary>
/// A single game piece
/// </summary>
public abstract class Piece
{
    /// <summary>The piece's in-game sprite</summary>
    public GameObject UnityObject { get; }

    /// <summary>True if this piece is controlled by the player</summary>
    public bool PlayerPiece { get; }

    /// <summary>The board this piece is on</summary>
    public Board Board { get; set; }

    /// <summary>The piece's coordinates on the board</summary>
    public Vector2Int Space { get; set; }

    /// <summary>Base constructor for Piece abstractions</summary>
    /// <param name="unityObject">The unity object behind this piece</param>
    /// <param name="playerPiece">True if this Piece is controlled by the player</param>
    public Piece(GameObject unityObject, bool playerPiece)
    {
        UnityObject = unityObject;
        PlayerPiece = playerPiece;
    }

    /// <summary>Enacts this piece's move for a single turn</summary>
    public abstract void Move();

    /// <summary>Destroys this piece</summary>
    public void Destroy()
    {
        Object.Destroy(UnityObject);
    }
}
