using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A chess queen
/// </summary>
public class Queen : Piece
{
    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        Vector2Int newSpace = new(Space.x, Space.y);

        // Get the possible moves this Queen can make
        List<Vector2Int> possibleMoves = new();
        List<Vector2Int> possibleCaptures = new();
        GetChoicesInDirection(0, 1, 100, possibleMoves, possibleCaptures);
        GetChoicesInDirection(1, 1, 100, possibleMoves, possibleCaptures);
        GetChoicesInDirection(1, 0, 100, possibleMoves, possibleCaptures);
        GetChoicesInDirection(1, -1, 100, possibleMoves, possibleCaptures);
        GetChoicesInDirection(0, -1, 100, possibleMoves, possibleCaptures);
        GetChoicesInDirection(-1, -1, 100, possibleMoves, possibleCaptures);
        GetChoicesInDirection(-1, 0, 100, possibleMoves, possibleCaptures);
        GetChoicesInDirection(-1, 1, 100, possibleMoves, possibleCaptures);

        // Capture a new piece if possible
        if (possibleCaptures.Count > 0) newSpace = possibleCaptures[Random.Range(0, possibleCaptures.Count)];

        // Otherwise move if possible
        else if (possibleMoves.Count > 0) newSpace = possibleMoves[Random.Range(0, possibleMoves.Count)];

        // Move to the new space
        EnactTurn(newSpace);
    }
}
