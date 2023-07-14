using UnityEngine;

/// <summary>
/// A chess Pawn
/// </summary>
public class Pawn : Piece
{
    public override AssetGroup.Piece Kind { get { return AssetGroup.Piece.Pawn; } }

    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        Space newSpace = Space;

        // Get the possible moves this piece can make
        Space move = GetMoveSpace();
        Space leftAttack = GetLeftAttackSpace();
        Space rightAttack = GetRightAttackSpace();

        // Capture a piece if possible
        if ((leftAttack != null && leftAttack.HasEnemy(IsPlayer)) && (rightAttack != null && rightAttack.HasEnemy(IsPlayer)))
        {
            // Choose randomly if both options are available
            if (Random.Range(0, 2) == 1) newSpace = leftAttack;
            else newSpace = rightAttack;
        }
        else if (leftAttack != null && leftAttack.HasEnemy(IsPlayer)) newSpace = leftAttack;
        else if (rightAttack != null && rightAttack.HasEnemy(IsPlayer)) newSpace = rightAttack;
        else
        {
            // Otherwise move if possible
            if (move != null && move.IsPassable(this)) newSpace = move;
        }

        // Move to the new space
        newSpace.MoveOnto(this);

        // If the new space is at the board edge, upgrade to a queen
        int edge = IsPlayer ? Board.Height - 1 : 0;
        if (newSpace.Y == edge) Transform = AssetGroup.Piece.Queen;
    }

    /// <summary>Gets the space in front of the Pawn</summary>
    /// <returns>The space's coordinates</returns>
    private Space GetMoveSpace()
    {
        Vector2Int position;
        if (IsPlayer) position = new Vector2Int(Space.X, Space.Y + 1);
        else position = new Vector2Int(Space.X, Space.Y - 1);
        return Board.GetSpace(position);
    }

    /// <summary>Gets the left-diagonal space in front of the Pawn</summary>
    /// <returns>The space's coordinates</returns>
    private Space GetLeftAttackSpace()
    {
        Vector2Int position;
        if (IsPlayer) position = new Vector2Int(Space.X - 1, Space.Y + 1);
        else position = new Vector2Int(Space.X - 1, Space.Y - 1);
        return Board.GetSpace(position);
    }

    /// <summary>Gets the right-diagonal space in front of the Pawn</summary>
    /// <returns>The space's coordinates</returns>
    private Space GetRightAttackSpace()
    {
        Vector2Int position;
        if (IsPlayer) position = new Vector2Int(Space.X + 1, Space.Y + 1);
        else position = new Vector2Int(Space.X + 1, Space.Y - 1);
        return Board.GetSpace(position);
    }
}
