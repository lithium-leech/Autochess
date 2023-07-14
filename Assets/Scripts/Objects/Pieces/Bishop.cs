﻿using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A chess bishop
/// </summary>
public class Bishop : Piece
{
    public override AssetGroup.Piece Kind { get { return AssetGroup.Piece.Bishop; } }

    public override void TakeTurn()
    {
        // Assume initially that the piece cannot move
        IList<Space> path = new List<Space>() { Space };

        // Get the possible moves this piece can make
        IList<IList<Space>> possibleMoves = new List<IList<Space>>();
        IList<IList<Space>> possibleCaptures = new List<IList<Space>>();
        GetChoicesInDirection(1, 1, 100, possibleMoves, possibleCaptures);
        GetChoicesInDirection(1, -1, 100, possibleMoves, possibleCaptures);
        GetChoicesInDirection(-1, -1, 100, possibleMoves, possibleCaptures);
        GetChoicesInDirection(-1, 1, 100, possibleMoves, possibleCaptures);

        // Capture a new piece if possible
        if (possibleCaptures.Count > 0) path = possibleCaptures[Random.Range(0, possibleCaptures.Count)];

        // Otherwise move if possible
        else if (possibleMoves.Count > 0) path = possibleMoves[Random.Range(0, possibleMoves.Count)];

        // Move to the new space
        StartMove(path);
    }
}
