using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Xiangqi chess piece
/// </summary>
public class Xiang : Piece
{
    public override AssetGroup.Piece Kind => AssetGroup.Piece.Xiang;
    
    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        IList<Vector2Int> path = new List<Vector2Int>() { Space.Coordinates };

        // Construct the initial paths
        IList<Vector2Int> ur = new List<Vector2Int>() { Space.Coordinates, Space.Coordinates + new Vector2Int(1, 1) };
        IList<Vector2Int> dr = new List<Vector2Int>() { Space.Coordinates, Space.Coordinates + new Vector2Int(1, -1) };
        IList<Vector2Int> dl = new List<Vector2Int>() { Space.Coordinates, Space.Coordinates + new Vector2Int(-1, -1) };
        IList<Vector2Int> ul = new List<Vector2Int>() { Space.Coordinates, Space.Coordinates + new Vector2Int(-1, 1) };

        // Get the possible moves and captures this piece can make
        IList<IList<Vector2Int>> moves = new List<IList<Vector2Int>>();
        IList<IList<Vector2Int>> captures = new List<IList<Vector2Int>>();
        if (IsValidPath(ur, false)) AddLinearPaths(ur, 1, 1, 1, false, moves, captures);
        if (IsValidPath(dr, false)) AddLinearPaths(dr, 1, -1, 1, false, moves, captures);
        if (IsValidPath(dl, false)) AddLinearPaths(dl, -1, -1, 1, false, moves, captures);
        if (IsValidPath(ul, false)) AddLinearPaths(ul, -1, 1, 1, false, moves, captures);

        // Capture a piece if possible
        if (captures.Count > 0) path = captures[Random.Range(0, captures.Count)];

        // Otherwise move if possible
        else if (moves.Count > 0) path = moves[Random.Range(0, moves.Count)];

        // Move to the new space
        EnactTurn(path);
    }
}
