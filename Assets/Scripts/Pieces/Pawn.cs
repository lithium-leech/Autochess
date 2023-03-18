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

        // Get the possible moves this piece can make
        Vector2Int move = GetMoveSpace();
        Vector2Int leftAttack = GetLeftAttackSpace();
        Vector2Int rightAttack = GetRightAttackSpace();

        // Capture a piece if possible
        if (Board.HasEnemy(IsPlayerPiece, leftAttack) && Board.HasEnemy(IsPlayerPiece, rightAttack))
        {
            // Choose randomly if both options are available
            if (Random.Range(0, 2) == 1) newSpace = leftAttack;
            else newSpace = rightAttack;
        }
        else if (Board.HasEnemy(IsPlayerPiece, leftAttack)) newSpace = leftAttack;
        else if (Board.HasEnemy(IsPlayerPiece, rightAttack)) newSpace = rightAttack;
        else
        {
            // Otherwise move if possible
            if (!Board.HasPiece(move) && Board.OnBoard(move)) newSpace = move;
        }

        // Move to the new space
        EnactTurn(newSpace);

        // If the new space is at the board edge, upgrade to a queen
        int edge = IsPlayerPiece ? Board.Height - 1 : 0;
        if (newSpace.y == edge)
        {
            Transform = AssetGroup.Piece.Queen;
        }
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
