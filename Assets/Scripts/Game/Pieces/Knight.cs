using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A classic chess piece
/// </summary>
public class Knight : Piece
{
    public override AssetGroup.Piece Kind => AssetGroup.Piece.Knight;

    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        IList<Vector2Int> path = new List<Vector2Int>() { Space.Coordinates };

        // Construct the initial paths
        IList<Vector2Int> u = new List<Vector2Int>() { Space.Coordinates, Space.Coordinates + new Vector2Int(0, 1), Space.Coordinates + new Vector2Int(0, 2) };
        IList<Vector2Int> r = new List<Vector2Int>() { Space.Coordinates, Space.Coordinates + new Vector2Int(1, 0), Space.Coordinates + new Vector2Int(2, 0) };
        IList<Vector2Int> d = new List<Vector2Int>() { Space.Coordinates, Space.Coordinates + new Vector2Int(0, -1), Space.Coordinates + new Vector2Int(0, -2) };
        IList<Vector2Int> l = new List<Vector2Int>() { Space.Coordinates, Space.Coordinates + new Vector2Int(-1, 0), Space.Coordinates + new Vector2Int(-2, 0) };

        // Get the possible moves and captures this piece can make
        IList<IList<Vector2Int>> moves = new List<IList<Vector2Int>>();
        IList<IList<Vector2Int>> captures = new List<IList<Vector2Int>>();
        if (IsValidPath(u, true)) AddHorizontalPaths(u, 1, 1, true, moves, captures);
        if (IsValidPath(r, true)) AddVerticalPaths(r, 1, 1, true, moves, captures);
        if (IsValidPath(d, true)) AddHorizontalPaths(d, 1, 1, true, moves, captures);
        if (IsValidPath(l, true)) AddVerticalPaths(l, 1, 1, true, moves, captures);

        // Capture a piece if possible
        if (captures.Count > 0) path = captures[Random.Range(0, captures.Count)];

        // Otherwise move if possible
        else if (moves.Count > 0) path = moves[Random.Range(0, moves.Count)];

        // Move to the new space
        EnactTurn(path);
    }
}
