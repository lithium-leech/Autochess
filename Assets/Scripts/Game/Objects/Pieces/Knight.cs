using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A chess knight
/// </summary>
public class Knight : Piece
{
    public override AssetGroup.Piece Kind => AssetGroup.Piece.Knight;

    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        IList<Space> path = new List<Space>() { Space };

        // Get the possible moves and captures this piece can make
        IList<IList<Space>> moves = new List<IList<Space>>();
        IList<IList<Space>> captures = new List<IList<Space>>();
        AddKnightPaths(1, 2, 1, true, moves, captures);

        // Capture a piece if possible
        if (captures.Count > 0) path = captures[Random.Range(0, captures.Count)];

        // Otherwise move if possible
        else if (moves.Count > 0) path = moves[Random.Range(0, moves.Count)];

        // Move to the new space
        EnactTurn(path);
    }
}
