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
        Vector2Int newSpace = new Vector2Int(Space.x, Space.y);

        // Get the spaces this piece can move
        List<Vector2Int> possibleSpaces = new List<Vector2Int>();
        possibleSpaces.Add(new Vector2Int(Space.x + 1, Space.y + 2));
        possibleSpaces.Add(new Vector2Int(Space.x + 2, Space.y + 1));
        possibleSpaces.Add(new Vector2Int(Space.x + 2, Space.y - 1));
        possibleSpaces.Add(new Vector2Int(Space.x + 1, Space.y - 2));
        possibleSpaces.Add(new Vector2Int(Space.x - 1, Space.y - 2));
        possibleSpaces.Add(new Vector2Int(Space.x - 2, Space.y - 1));
        possibleSpaces.Add(new Vector2Int(Space.x - 2, Space.y + 1));
        possibleSpaces.Add(new Vector2Int(Space.x - 1, Space.y + 2));

        // Get the possible moves this piece can make
        List<Vector2Int> possibleMoves = new List<Vector2Int>();
        List<Vector2Int> possibleCaptures = new List<Vector2Int>();
        foreach (Vector2Int space in possibleSpaces)
        {
            if (Board.OnBoard(space))
            {
                if (Board.HasEnemy(IsPlayer, space)) possibleCaptures.Add(space);
                else if (!Board.GetPiece(space)) possibleMoves.Add(space);
            }
        }

        // Capture a new piece if possible
        if (possibleCaptures.Count > 0) newSpace = possibleCaptures[Random.Range(0, possibleCaptures.Count)];

        // Otherwise move if possible
        else if (possibleMoves.Count > 0) newSpace = possibleMoves[Random.Range(0, possibleMoves.Count)];

        // Move to the new space
        EnactTurn(newSpace);
    }
}
