using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A chess rook
/// </summary>
public class Rook : Piece
{
    public override AssetGroup.Piece Kind { get { return AssetGroup.Piece.Rook; } }

    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        Vector2Int newSpace = new Vector2Int(Space.x, Space.y);

        // Get the possible moves this piece can make
        List<Vector2Int> possibleMoves = new List<Vector2Int>();
        List<Vector2Int> possibleCaptures = new List<Vector2Int>();
        GetChoicesInDirection(0, 1, 100, possibleMoves, possibleCaptures);
        GetChoicesInDirection(1, 0, 100, possibleMoves, possibleCaptures);
        GetChoicesInDirection(0, -1, 100, possibleMoves, possibleCaptures);
        GetChoicesInDirection(-1, 0, 100, possibleMoves, possibleCaptures);

        // Capture a new piece if possible
        if (possibleCaptures.Count > 0) newSpace = possibleCaptures[Random.Range(0, possibleCaptures.Count)];

        // Otherwise move if possible
        else if (possibleMoves.Count > 0) newSpace = possibleMoves[Random.Range(0, possibleMoves.Count)];

        // Move to the new space
        EnactTurn(newSpace);
    }
}
