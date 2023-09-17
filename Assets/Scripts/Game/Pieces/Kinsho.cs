using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A Shogi chess piece
/// </summary>
public class Kinsho : Piece
{
    public override AssetGroup.Piece Kind => AssetGroup.Piece.Kinsho;
    
    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        IList<Vector2Int> path = new List<Vector2Int>() { Space.Coordinates };

        // Get the possible moves and captures this piece can make
        IList<IList<Vector2Int>> moves = new List<IList<Vector2Int>>();
        IList<IList<Vector2Int>> captures = new List<IList<Vector2Int>>();
        AddOrthogonalPaths(path, 1, 1, false, moves, captures);
        int yi = IsPlayer ? 1 : -1;
        AddLinearPaths(path, -1, yi, 1, false, moves, captures);
        AddLinearPaths(path, 1, yi, 1, false, moves, captures);

        // Capture a piece if possible
        if (captures.Count > 0) path = captures[Random.Range(0, captures.Count)];

        // Otherwise move if possible
        else if (moves.Count > 0) path = moves[Random.Range(0, moves.Count)];

        // Move to the new space
        EnactTurn(path);
    }
}
