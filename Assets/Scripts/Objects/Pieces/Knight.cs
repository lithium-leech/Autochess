using System.Collections.Generic;
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
        Space newSpace = Space;

        // Get the spaces this piece can move
        List<Space> possibleSpaces = new List<Space>();
        possibleSpaces.Add(Board.GetSpace(new Vector2Int(Space.X + 1, Space.Y + 2)));
        possibleSpaces.Add(Board.GetSpace(new Vector2Int(Space.X + 2, Space.Y + 1)));
        possibleSpaces.Add(Board.GetSpace(new Vector2Int(Space.X + 2, Space.Y - 1)));
        possibleSpaces.Add(Board.GetSpace(new Vector2Int(Space.X + 1, Space.Y - 2)));
        possibleSpaces.Add(Board.GetSpace(new Vector2Int(Space.X - 1, Space.Y - 2)));
        possibleSpaces.Add(Board.GetSpace(new Vector2Int(Space.X - 2, Space.Y - 1)));
        possibleSpaces.Add(Board.GetSpace(new Vector2Int(Space.X - 2, Space.Y + 1)));
        possibleSpaces.Add(Board.GetSpace(new Vector2Int(Space.X - 1, Space.Y + 2)));

        // Get the possible moves this piece can make
        List<Space> possibleMoves = new List<Space>();
        List<Space> possibleCaptures = new List<Space>();
        foreach (Space space in possibleSpaces)
        {
            if (space != null)
            {
                if (space.HasEnemy(IsPlayer)) possibleCaptures.Add(space);
                else if (space.IsPassable(this)) possibleMoves.Add(space);
            }
        }

        // Capture a new piece if possible
        if (possibleCaptures.Count > 0) newSpace = possibleCaptures[Random.Range(0, possibleCaptures.Count)];

        // Otherwise move if possible
        else if (possibleMoves.Count > 0) newSpace = possibleMoves[Random.Range(0, possibleMoves.Count)];

        // Move to the new space
        newSpace.MoveOnto(this);
    }
}
