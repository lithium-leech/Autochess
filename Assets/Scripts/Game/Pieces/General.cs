using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A custom chess piece
/// </summary>
public class General : Piece
{
    public override AssetGroup.Piece Kind => AssetGroup.Piece.General;
    
    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        IList<Space> path = new List<Space>() { Space };

        // Get the possible moves this piece can make
        IList<IList<Space>> moves = new List<IList<Space>>();
        AddRookPaths(1, 2, false, moves, new List<IList<Space>>());
        AddBishopPaths(1, 2, false, moves, new List<IList<Space>>());

        // Get the possible captures this piece can make
        IList<IList<Space>> captures = new List<IList<Space>>();
        AddRookPaths(1, 1, false, new List<IList<Space>>(), captures);
        AddBishopPaths(1, 1, false, new List<IList<Space>>(), captures);

        // Capture a piece if possible
        if (captures.Count > 0) path = captures[Random.Range(0, captures.Count)];

        // Otherwise move if possible
        else if (moves.Count > 0) path = moves[Random.Range(0, moves.Count)];

        // Move to the new space
        EnactTurn(path);
    }
}
