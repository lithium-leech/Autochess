using System.Collections.Generic;
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
        IList<Space> path = new List<Space>() { Space };

        // Get the possible moves and captures this piece can make
        IList<IList<Space>> moves = new List<IList<Space>>();
        IList<IList<Space>> captures = new List<IList<Space>>();
        AddRookPaths(1, 4, false, moves, captures);
        AddBishopPaths(1, 4, false, moves, captures);
        AddKnightPaths(1, 2, 1, false, moves, captures);
        AddKnightPaths(1, 3, 1, false, moves, captures);

        // Capture a new piece if possible
        if (captures.Count > 0) path = captures[Random.Range(0, captures.Count)];

        // Otherwise move if possible
        else if (moves.Count > 0) path = moves[Random.Range(0, moves.Count)];

        // Move to the new space
        EnactTurn(path);
    }
}
