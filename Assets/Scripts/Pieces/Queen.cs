using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A chess queen
/// </summary>
public class Queen : Piece
{
    /// <summary>Creates a new instance of a Queen</summary>
    /// <param name="unityObject">The unity object behind this piece</param>
    /// <param name="playerPiece">True if this Piece is controlled by the player</param>

    public Queen(GameObject unityObject, bool playerPiece) : base(unityObject, playerPiece) { }

    public override void Move()
    {
        // Assume initially that the piece cannot move
        Vector2Int newSpace = new(Space.x, Space.y);

        // Get the possible moves this Queen can make
        List<Vector2Int> possibleMoves = new();
        List<Vector2Int> possibleCaptures = new();
        GetChoicesInDirection(0, 1, possibleMoves, possibleCaptures);
        GetChoicesInDirection(1, 1, possibleMoves, possibleCaptures);
        GetChoicesInDirection(1, 0, possibleMoves, possibleCaptures);
        GetChoicesInDirection(1, -1, possibleMoves, possibleCaptures);
        GetChoicesInDirection(0, -1, possibleMoves, possibleCaptures);
        GetChoicesInDirection(-1, -1, possibleMoves, possibleCaptures);
        GetChoicesInDirection(-1, 0, possibleMoves, possibleCaptures);
        GetChoicesInDirection(-1, 1, possibleMoves, possibleCaptures);

        // Capture a new piece if possible
        if (possibleCaptures.Count > 0) newSpace = possibleCaptures[RND.Next(possibleCaptures.Count)];
        
        // Otherwise move if possible
        else if (possibleMoves.Count > 0) newSpace = possibleMoves[RND.Next(possibleMoves.Count)];

        // Move to the new space
        Board.Move(this, newSpace);
    }
}
