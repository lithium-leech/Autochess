using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes a 5x5 board
/// </summary>
public class SmallBoardBuilder : BoardBuilder
{
    /// <summary>Creates a new instance of a SmallBoardBuilder</summary>
    /// <param name="game">The game to build a board in</param>
    public SmallBoardBuilder(Game game) => Game = game;

    public override Board Build()
    {
        // Create a new board
        Board board = new()
        {
            Game = Game,
            Width = 5,
            Height = 5,
            PlayerRows = 2,
            EnemyRows = 2,
            PlayerPieces = new List<Piece>(),
            EnemyPieces = new List<Piece>(),
            CornerBL = new Vector2(-2.5f, 0.5f)
        };
        board.CornerTR = board.CornerBL + new Vector2(board.Width, board.Height);

        // Create a square array of spaces
        board.Spaces = new Space[board.Width,board.Height];
        for (int x = 0; x < board.Width; x++)
        for (int y = 0; y < board.Height; y++)
        {
            board.Spaces[x,y] = new Space(board, x, y);
        }

        // Change the top and bottom rows to promotion spaces
        for (int x = 0; x < board.Width; x++)
        {
            board.Spaces[x,0].IsEnemyPromotion = true;
            board.Spaces[x,board.Height-1].IsPlayerPromotion = true;
        }

        // Create tiles for the configured spaces
        CreateTiles(board, false);

        // Return the finished board
        return board;
    }
}
