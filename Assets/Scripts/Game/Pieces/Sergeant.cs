using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A custom chess piece
/// </summary>
public class Sergeant : Piece
{
    public override AssetGroup.Piece Kind => AssetGroup.Piece.Sergeant;
    
    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        IList<Vector2Int> path = new List<Vector2Int>() { Space.Coordinates };

        // Get the possible moves and captures this piece can make
        IList<IList<Vector2Int>> moves = new List<IList<Vector2Int>>();
        IList<IList<Vector2Int>> captures = new List<IList<Vector2Int>>();
        AddOrthogonalPaths(path, 1, 3, true, moves, captures);

        // Capture a piece if possible
        if (captures.Count > 0) path = captures[Random.Range(0, captures.Count)];

        // Otherwise move if possible
        else if (moves.Count > 0) path = moves[Random.Range(0, moves.Count)];

        // Move to the new space
        EnactTurn(path);
    }
}
