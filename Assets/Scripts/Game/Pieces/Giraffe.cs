using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Tamerlane chess piece
/// </summary>
public class Giraffe : Piece
{
    public override AssetGroup.Piece Kind => AssetGroup.Piece.Giraffe;
    
    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        IList<Vector2Int> path = new List<Vector2Int>() { Space.Coordinates };

        // Construct the initial paths
        IList<Vector2Int> uru = new List<Vector2Int>() { Space.Coordinates, Space.Coordinates + new Vector2Int(1, 1), Space.Coordinates + new Vector2Int(1, 2) };
        IList<Vector2Int> urr = new List<Vector2Int>() { Space.Coordinates, Space.Coordinates + new Vector2Int(1, 1), Space.Coordinates + new Vector2Int(2, 1) };
        IList<Vector2Int> drr = new List<Vector2Int>() { Space.Coordinates, Space.Coordinates + new Vector2Int(1, -1), Space.Coordinates + new Vector2Int(2, -1) };
        IList<Vector2Int> drd = new List<Vector2Int>() { Space.Coordinates, Space.Coordinates + new Vector2Int(1, -1), Space.Coordinates + new Vector2Int(1, -2) };
        IList<Vector2Int> dld = new List<Vector2Int>() { Space.Coordinates, Space.Coordinates + new Vector2Int(-1, -1), Space.Coordinates + new Vector2Int(-1, -2) };
        IList<Vector2Int> dll = new List<Vector2Int>() { Space.Coordinates, Space.Coordinates + new Vector2Int(-1, -1), Space.Coordinates + new Vector2Int(-2, -1) };
        IList<Vector2Int> ull = new List<Vector2Int>() { Space.Coordinates, Space.Coordinates + new Vector2Int(-1, 1), Space.Coordinates + new Vector2Int(-2, 1) };
        IList<Vector2Int> ulu = new List<Vector2Int>() { Space.Coordinates, Space.Coordinates + new Vector2Int(-1, 1), Space.Coordinates + new Vector2Int(-1, 2) };

        // Get the possible moves and captures this piece can make
        IList<IList<Vector2Int>> moves = new List<IList<Vector2Int>>();
        IList<IList<Vector2Int>> captures = new List<IList<Vector2Int>>();
        if (IsValidPath(uru, false)) AddLinearPaths(uru, 0, 1, 100, false, moves, captures);
        if (IsValidPath(urr, false)) AddLinearPaths(urr, 1, 0, 100, false, moves, captures);
        if (IsValidPath(drr, false)) AddLinearPaths(drr, 1, 0, 100, false, moves, captures);
        if (IsValidPath(drd, false)) AddLinearPaths(drd, 0, -1, 100, false, moves, captures);
        if (IsValidPath(dld, false)) AddLinearPaths(dld, 0, -1, 100, false, moves, captures);
        if (IsValidPath(dll, false)) AddLinearPaths(dll, -1, 0, 100, false, moves, captures);
        if (IsValidPath(ull, false)) AddLinearPaths(ull, -1, 0, 100, false, moves, captures);
        if (IsValidPath(ulu, false)) AddLinearPaths(ulu, 0, 1, 100, false, moves, captures);

        // Capture a piece if possible
        if (captures.Count > 0) path = captures[Random.Range(0, captures.Count)];

        // Otherwise move if possible
        else if (moves.Count > 0) path = moves[Random.Range(0, moves.Count)];

        // Move to the new space
        EnactTurn(path);
    }
}
