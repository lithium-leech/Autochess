using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A Shogi chess piece
/// </summary>
public class Fuhyo : Piece
{
    public override AssetGroup.Piece Kind => AssetGroup.Piece.Fuhyo;
    
    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        IList<Vector2Int> path = new List<Vector2Int>() { Space.Coordinates };

        // Get the possible moves and captures this piece can make
        IList<IList<Vector2Int>> moves = new List<IList<Vector2Int>>();
        IList<IList<Vector2Int>> captures = new List<IList<Vector2Int>>();
        int yi = IsPlayer ? 1 : -1;
        AddLinearPaths(path, 0, yi, 1, false, moves, captures);

        // Capture a piece if possible
        if (captures.Count > 0) path = captures[Random.Range(0, captures.Count)];

        // Otherwise move if possible
        else if (moves.Count > 0) path = moves[Random.Range(0, moves.Count)];

        // Move to the new space
        EnactTurn(path);
        
        // Check for promotion
        Space last = Board.GetSpace(path.Last());
        if ((IsPlayer && last.IsPlayerPromotion) || (!IsPlayer && last.IsEnemyPromotion))
            Transform = AssetGroup.Piece.Tokin;
    }
}
