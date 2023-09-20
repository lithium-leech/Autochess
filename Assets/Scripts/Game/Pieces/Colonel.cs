using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A custom chess piece
/// </summary>
public class Colonel : Piece
{
    public override AssetGroup.Piece Kind => AssetGroup.Piece.Colonel;
    
    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        IList<Vector2Int> path = new List<Vector2Int>() { Space.Coordinates };

        // Get the possible moves and captures this piece can make
        IList<IList<Vector2Int>> moves1 = new List<IList<Vector2Int>>();
        IList<IList<Vector2Int>> moves2 = new List<IList<Vector2Int>>();
        IList<IList<Vector2Int>> moves3 = new List<IList<Vector2Int>>();
        IList<IList<Vector2Int>> moves4 = new List<IList<Vector2Int>>();
        IList<IList<Vector2Int>> captures = new List<IList<Vector2Int>>();
        AddOrthogonalPaths(path, 1, 1, false, moves1, captures);
        foreach (IList<Vector2Int> move in moves1) AddOrthogonalPaths(move, 1, 1, false, moves2, captures);
        foreach (IList<Vector2Int> move in moves2) AddOrthogonalPaths(move, 1, 1, false, moves3, captures);
        foreach (IList<Vector2Int> move in moves3) AddOrthogonalPaths(move, 1, 1, false, moves4, captures);
        IList<IList<Vector2Int>> moves = moves1.Concat(moves2).Concat(moves3).Concat(moves4).ToList();

        // Capture a new piece if possible
        if (captures.Count > 0) path = captures[Random.Range(0, captures.Count)];

        // Otherwise move if possible
        else if (moves.Count > 0) path = moves[Random.Range(0, moves.Count)];

        // Move to the new space
        EnactTurn(path);
    }
}
