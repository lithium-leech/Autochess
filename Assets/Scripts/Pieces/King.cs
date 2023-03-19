using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A chess king
/// </summary>
public class King : Piece
{
    public override AssetGroup.Piece Kind { get { return AssetGroup.Piece.King; } }

    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        Vector2Int newSpace = new(Space.x, Space.y);

        // Get the possible moves this piece can make
        List<Vector2Int> possibleMoves = new();
        List<Vector2Int> possibleCaptures = new();
        GetChoicesInDirection(0, 1, 1, possibleMoves, possibleCaptures);
        GetChoicesInDirection(1, 1, 1, possibleMoves, possibleCaptures);
        GetChoicesInDirection(1, 0, 1, possibleMoves, possibleCaptures);
        GetChoicesInDirection(1, -1, 1, possibleMoves, possibleCaptures);
        GetChoicesInDirection(0, -1, 1, possibleMoves, possibleCaptures);
        GetChoicesInDirection(-1, -1, 1, possibleMoves, possibleCaptures);
        GetChoicesInDirection(-1, 0, 1, possibleMoves, possibleCaptures);
        GetChoicesInDirection(-1, 1, 1, possibleMoves, possibleCaptures);

        // Capture a new piece if possible
        if (possibleCaptures.Count > 0) newSpace = possibleCaptures[Random.Range(0, possibleCaptures.Count)];

        // Otherwise move if possible
        else if (possibleMoves.Count > 0) newSpace = possibleMoves[Random.Range(0, possibleMoves.Count)];

        // Move to the new space
        EnactTurn(newSpace);
    }
}
