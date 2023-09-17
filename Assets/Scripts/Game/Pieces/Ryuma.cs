using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Shogi chess piece
/// </summary>
public class Ryuma : Piece
{
    public override AssetGroup.Piece Kind => AssetGroup.Piece.Ryuma;
    
    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        IList<Vector2Int> path = new List<Vector2Int>() { Space.Coordinates };

        // Get the possible moves and captures this piece can make
        IList<IList<Vector2Int>> moves = new List<IList<Vector2Int>>();
        IList<IList<Vector2Int>> captures = new List<IList<Vector2Int>>();
        AddDiagonalPaths(path, 1, 100, false, moves, captures);
        AddOrthogonalPaths(path, 1, 1, false, moves, captures);

        // Capture a piece if possible
        if (captures.Count > 0) path = captures[Random.Range(0, captures.Count)];

        // Otherwise move if possible
        else if (moves.Count > 0) path = moves[Random.Range(0, moves.Count)];

        // Move to the new space
        EnactTurn(path);
    }
}
