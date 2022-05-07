using UnityEngine;

/// <summary>
/// A chess Pawn
/// </summary>
public class Pawn : Piece
{
    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        Vector2Int newSpace = new(Space.x, Space.y);

        // Get the possible moves this Pawn can make
        Vector2Int move = GetMoveSpace();
        Vector2Int leftAttack = GetLeftAttackSpace();
        Vector2Int rightAttack = GetRightAttackSpace();

        // Capture a piece if possible
        if (HasEnemy(leftAttack) && HasEnemy(rightAttack))
        {
            // Choose randomly if both options are available
            if (Random.Range(0, 2) == 1) newSpace = leftAttack;
            else newSpace = rightAttack;
        }
        else if (HasEnemy(leftAttack)) newSpace = leftAttack;
        else if (HasEnemy(rightAttack)) newSpace = rightAttack;
        else
        {
            // Otherwise move if possible
            if (!HasPiece(move) && OnBoard(move)) newSpace = move;
        }

        // Move to the new space
        Board.Move(this, newSpace);
    }

    /// <summary>Gets the space in front of the Pawn</summary>
    /// <returns>The space's coordinates</returns>
    private Vector2Int GetMoveSpace()
    {
        if (IsPlayerPiece) return new Vector2Int(Space.x, Space.y + 1);
        else return new Vector2Int(Space.x, Space.y - 1);
    }

    /// <summary>Gets the left-diagonal space in front of the Pawn</summary>
    /// <returns>The space's coordinates</returns>
    private Vector2Int GetLeftAttackSpace()
    {
        if (IsPlayerPiece) return new Vector2Int(Space.x - 1, Space.y + 1);
        else return new Vector2Int(Space.x - 1, Space.y - 1);
    }

    /// <summary>Gets the right-diagonal space in front of the Pawn</summary>
    /// <returns>The space's coordinates</returns>
    private Vector2Int GetRightAttackSpace()
    {
        if (IsPlayerPiece) return new Vector2Int(Space.x + 1, Space.y + 1);
        else return new Vector2Int(Space.x + 1, Space.y - 1);
    }
}
