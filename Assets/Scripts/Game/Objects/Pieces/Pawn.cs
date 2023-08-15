using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A chess Pawn
/// </summary>
public class Pawn : Piece
{
    public override AssetGroup.Piece Kind => AssetGroup.Piece.Pawn;
    
    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        IList<Space> path = new List<Space>() { Space };

        // Get the spaces in front of this piece
        Space move = GetMoveSpace();
        Space leftAttack = GetLeftAttackSpace();
        Space rightAttack = GetRightAttackSpace();

        // Capture a piece if possible
        if ((leftAttack != null && leftAttack.HasCapturable(this)) && (rightAttack != null && rightAttack.HasCapturable(this)))
        {
            // Choose randomly if both options are available
            if (Random.Range(0, 2) == 1) path = new List<Space>() { Space, leftAttack };
            else path = new List<Space>() { Space, rightAttack };
        }
        else if (leftAttack != null && leftAttack.HasCapturable(this)) path = new List<Space>() { Space, leftAttack };
        else if (rightAttack != null && rightAttack.HasCapturable(this)) path = new List<Space>() { Space, rightAttack };
        else
        {
            // Otherwise move if possible
            if (move != null && move.IsEnterable(this)) path = new List<Space>() { Space, move };
        }

        // Move to the new space
        EnactTurn(path);

        // If the new space is at the board edge, upgrade to a queen
        int edge = IsPlayer ? Board.Height - 1 : 0;
        if (path.Last().Y == edge) Transform = AssetGroup.Piece.Queen;
    }

    /// <summary>Gets the space in front of the Pawn</summary>
    /// <returns>The space</returns>
    private Space GetMoveSpace()
    {
        Vector2Int coordinate;
        if (IsPlayer) coordinate = Space.Coordinates + new Vector2Int(0, 1);
        else coordinate = Space.Coordinates + new Vector2Int(0, -1);
        return Board.GetSpace(coordinate);
    }

    /// <summary>Gets the left-diagonal space in front of the Pawn</summary>
    /// <returns>The space</returns>
    private Space GetLeftAttackSpace()
    {
        Vector2Int coordinate;
        if (IsPlayer) coordinate = Space.Coordinates + new Vector2Int(-1, 1);
        else coordinate = Space.Coordinates + new Vector2Int(-1, -1);
        return Board.GetSpace(coordinate);
    }

    /// <summary>Gets the right-diagonal space in front of the Pawn</summary>
    /// <returns>The space</returns>
    private Space GetRightAttackSpace()
    {
        Vector2Int coordinate;
        if (IsPlayer) coordinate = Space.Coordinates + new Vector2Int(1, 1);
        else coordinate = Space.Coordinates + new Vector2Int(1, -1);
        return Board.GetSpace(coordinate);
    }
}
