using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Tamerlane chess piece
/// </summary>
public class Ferz : Piece
{
    public override AssetGroup.Piece Kind => AssetGroup.Piece.Ferz;
    
    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        IList<Vector2Int> path = new List<Vector2Int>() { Space.Coordinates };

        // Get the possible moves this piece can make
        IList<IList<Vector2Int>> moves = new List<IList<Vector2Int>>();
        AddOrthogonalPaths(path, 1, 1, false, moves, new List<IList<Vector2Int>>());

        // Get the possible captures this piece can make
        IList<IList<Vector2Int>> captures = new List<IList<Vector2Int>>();
        AddOrthogonalPaths(path, 1, 2, false, new List<IList<Vector2Int>>(), captures);

        // Capture a piece if possible
        if (captures.Count > 0) path = captures[Random.Range(0, captures.Count)];

        // Otherwise move if possible
        else if (moves.Count > 0) path = moves[Random.Range(0, moves.Count)];

        // Move to the new space
        EnactTurn(path);
    }
}
