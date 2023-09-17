using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Xiangqi chess piece
/// </summary>
public class Ma : Piece
{
    public override AssetGroup.Piece Kind => AssetGroup.Piece.Ma;
    
    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        IList<Vector2Int> path = new List<Vector2Int>() { Space.Coordinates };

        // Construct the initial paths
        IList<Vector2Int> u = new List<Vector2Int>() { Space.Coordinates, Space.Coordinates + new Vector2Int(0, 1) };
        IList<Vector2Int> r = new List<Vector2Int>() { Space.Coordinates, Space.Coordinates + new Vector2Int(1, 0) };
        IList<Vector2Int> d = new List<Vector2Int>() { Space.Coordinates, Space.Coordinates + new Vector2Int(0, -1) };
        IList<Vector2Int> l = new List<Vector2Int>() { Space.Coordinates, Space.Coordinates + new Vector2Int(-1, 0) };

        // Get the possible moves and captures this piece can make
        IList<IList<Vector2Int>> moves = new List<IList<Vector2Int>>();
        IList<IList<Vector2Int>> captures = new List<IList<Vector2Int>>();
        if (IsValidPath(u, false))
        {
            AddLinearPaths(u, -1, 1, 1, false, moves, captures);
            AddLinearPaths(u, 1, 1, 1, false, moves, captures);
        }
        if (IsValidPath(r, false))
        {
            AddLinearPaths(r, 1, 1, 1, false, moves, captures);
            AddLinearPaths(r, 1, -1, 1, false, moves, captures);
        }
        if (IsValidPath(d, false))
        {
            AddLinearPaths(d, 1, -1, 1, false, moves, captures);
            AddLinearPaths(d, -1, -1, 1, false, moves, captures);
        }
        if (IsValidPath(l, false))
        {
            AddLinearPaths(l, -1, -1, 1, false, moves, captures);
            AddLinearPaths(l, -1, 1, 1, false, moves, captures);
        }

        // Capture a piece if possible
        if (captures.Count > 0) path = captures[Random.Range(0, captures.Count)];

        // Otherwise move if possible
        else if (moves.Count > 0) path = moves[Random.Range(0, moves.Count)];

        // Move to the new space
        EnactTurn(path);
    }
}
