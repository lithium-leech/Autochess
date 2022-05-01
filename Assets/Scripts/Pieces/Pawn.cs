using UnityEngine;

/// <summary>
/// A chess Pawn
/// </summary>
public class Pawn : Piece
{
    /// <summary>Creates a new instance of a Pawn</summary>
    /// <param name="unityObject">The unity object behind this piece</param>
    /// <param name="playerPiece">True if this Piece is controlled by the player</param>
    public Pawn(GameObject unityObject, bool playerPiece) : base(unityObject, playerPiece) {}

    public override void Move()
    {
        // Assume initially that the piece cannot move
        Vector2Int newSpace = new(Space.x, Space.y);

        // Get the possible moves this Pawn can make
        Vector2Int move = GetMoveSpace();
        Vector2Int leftAttack = GetLeftAttackSpace();
        Vector2Int rightAttack = GetRightAttackSpace();

        // Move if possible
        if (!HasPiece(move) && OnBoard(move)) newSpace = move;

        // Capture a piece if possible
        // (performed after move check so that it overwrites)
        if (HasPiece(leftAttack) && HasPiece(rightAttack))
        {
            // Choose randomly if both options are available
            if (Random.Range(0, 2) == 1) newSpace = leftAttack;
            else newSpace = rightAttack;
        }
        else if (HasPiece(leftAttack)) newSpace = leftAttack;
        else if (HasPiece(rightAttack)) newSpace = rightAttack;

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

    /// <summary>Checks if a space has a piece on it</summary>
    /// <param name="space">The space to check</param>
    /// <returns>True if the space has a piece on it</returns>
    private bool HasPiece(Vector2Int space)
    {
        if (OnBoard(space))
        {
            if (Board.Spaces[space.x, space.y] != null)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>Checks if a space is on the board</summary>
    /// <param name="space">The space to check</param>
    /// <returns>True if the space is on the board</returns>
    private bool OnBoard(Vector2Int space)
    {
        return space.x >= 0 && space.x < Board.Width || space.y >= 0 || space.y < Board.Height;
    }
}
