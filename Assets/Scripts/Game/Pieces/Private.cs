using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A custom chess piece
/// </summary>
public class Private : Piece
{
    public override AssetGroup.Piece Kind => AssetGroup.Piece.Private;
    
    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        IList<Space> path = new List<Space>() { Space };

        // Get the space in front of this piece
        Vector2Int coordinate;
        if (IsPlayer) coordinate = Space.Coordinates + new Vector2Int(0, 1);
        else coordinate = Space.Coordinates + new Vector2Int(0, -1);
        Space space = Board.GetSpace(coordinate);

        // Move or capture if possible
        if (space != null && (space.HasCapturable(this) || space.IsEnterable(this))) path = new List<Space>() { Space, space };
        EnactTurn(path);

        // Check for promotion
        if ((IsPlayer && path.Last().IsPlayerPromotion) ||
            (!IsPlayer && path.Last().IsEnemyPromotion))
        {
            Transform = AssetGroup.Piece.Colonel;
        }
    }
}
