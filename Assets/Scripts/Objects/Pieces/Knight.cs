using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A chess knight
/// </summary>
public class Knight : Piece
{
    public override AssetGroup.Piece Kind { get { return AssetGroup.Piece.Knight; } }

    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        IList<Space> path = new List<Space>() { Space };

        // Get all the paths this piece can make
        IList<IList<Space>> allPaths = new List<IList<Space>>();
        allPaths.Add(new List<Space>() { Space, Space.GetRelativeSpace(0, 1), Space.GetRelativeSpace(0, 2), Space.GetRelativeSpace(1, 2) });
        allPaths.Add(new List<Space>() { Space, Space.GetRelativeSpace(1, 0), Space.GetRelativeSpace(2, 0), Space.GetRelativeSpace(2, 1) });
        allPaths.Add(new List<Space>() { Space, Space.GetRelativeSpace(1, 0), Space.GetRelativeSpace(2, 0), Space.GetRelativeSpace(2, -1) });
        allPaths.Add(new List<Space>() { Space, Space.GetRelativeSpace(0, -1), Space.GetRelativeSpace(0, -2), Space.GetRelativeSpace(1, -2) });
        allPaths.Add(new List<Space>() { Space, Space.GetRelativeSpace(0, -1), Space.GetRelativeSpace(0, -2), Space.GetRelativeSpace(-1, -2) });
        allPaths.Add(new List<Space>() { Space, Space.GetRelativeSpace(-1, 0), Space.GetRelativeSpace(-2, 0), Space.GetRelativeSpace(-2, -1) });
        allPaths.Add(new List<Space>() { Space, Space.GetRelativeSpace(-1, 0), Space.GetRelativeSpace(-2, 0), Space.GetRelativeSpace(-2, 1) });
        allPaths.Add(new List<Space>() { Space, Space.GetRelativeSpace(0, 1), Space.GetRelativeSpace(0, 2), Space.GetRelativeSpace(-1, 2) });

        // Determine which paths are possible
        IList<IList<Space>> possibleMoves = new List<IList<Space>>();
        IList<IList<Space>> possibleCaptures = new List<IList<Space>>();
        foreach (IList<Space> option in allPaths)
        {
            Space space = option.Last();
            if (space != null)
            {
                if (space.HasCapturable(this)) possibleCaptures.Add(option);
                else if (space.IsEnterable(this)) possibleMoves.Add(option);
            }
        }

        // Capture a new piece if possible
        if (possibleCaptures.Count > 0) path = possibleCaptures[Random.Range(0, possibleCaptures.Count)];

        // Otherwise move if possible
        else if (possibleMoves.Count > 0) path = possibleMoves[Random.Range(0, possibleMoves.Count)];

        // Move to the new space
        EnactTurn(path);
    }
}
