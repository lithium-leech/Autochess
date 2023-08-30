using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the 8x2 side board below the game board
/// </summary>
public class SideBoardBuilder : BoardBuilder
{
    /// <summary>Creates a new instance of a SideBoardBuilder</summary>
    /// <param name="game">The game to build a board in</param>
    public SideBoardBuilder(Game game) => Game = game;

    public override Board Build()
    {
        // Create a new board
        Board board = new()
        {
            Game = Game,
            Width = 8,
            Height = 2,
            PlayerRows = 2,
            EnemyRows = 0,
            PlayerPieces = new List<Piece>(),
            EnemyPieces = new List<Piece>(),
            CornerBL = new Vector2(-4.0f, -6.0f),
        };
        board.CornerTR = board.CornerBL + new Vector2(board.Width, board.Height);

        // Create a square array of spaces
        board.Spaces = new Space[board.Width,board.Height];
        for (int x = 0; x < board.Width; x++)
        for (int y = 0; y < board.Height; y++)
        {
            board.Spaces[x,y] = new Space(board, x, y);
        }

        // Create tiles for the configured spaces
        CreateTiles(board, true);

        // Return the finished board
        return board;
    }
}
