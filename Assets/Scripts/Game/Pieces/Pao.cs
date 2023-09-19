using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Xiangqi chess piece
/// </summary>
public class Pao : Piece
{
    public override AssetGroup.Piece Kind => AssetGroup.Piece.Pao;
    
    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        IList<Vector2Int> path = new List<Vector2Int>() { Space.Coordinates };

        // Get the possible moves and captures this piece can make
        IList<IList<Vector2Int>> moves = new List<IList<Vector2Int>>();
        IList<IList<Vector2Int>> possibleCaptures = new List<IList<Vector2Int>>();
        AddOrthogonalPaths(path, 1, 100, false, moves, new List<IList<Vector2Int>>());
        AddOrthogonalPaths(path, 1, 100, true, new List<IList<Vector2Int>>(), possibleCaptures);
        
        // Find the valid captures
        IList<IList<Vector2Int>> captures = new List<IList<Vector2Int>>();
        foreach (IList<Vector2Int> capture in possibleCaptures)
        {
            bool screen = false;
            bool target = false;
            for (int i = 1; i < capture.Count; i++)
            {
                Vector2Int coordinates = capture[i];
                Space space = Board.GetSpace(coordinates);
                if (space != null)
                {
                    if (target) { target = false; break; }
                    if (screen && space.HasEnemy(this)) target = true;
                    if (!screen && !space.IsEnterable(this)) screen = true;
                }
                else if (!screen) screen = true;
                else { target = false; break; }
            }
            if (screen && target) captures.Add(capture);
        }

        // Capture a piece if possible
        if (captures.Count > 0) path = captures[Random.Range(0, captures.Count)];

        // Otherwise move if possible
        else if (moves.Count > 0) path = moves[Random.Range(0, moves.Count)];

        // Move to the new space
        EnactTurn(path);
    }
}
