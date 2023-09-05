using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes an 8x8 board with spaces removed on the left and right side
/// </summary>
public class HourglassBoardBuilder : BoardBuilder
{
    /// <summary>Creates a new instance of a HourglassBoardBuilder</summary>
    /// <param name="game">The game to build a board in</param>
    public HourglassBoardBuilder(Game game) => Game = game;

    public override Board Build()
    {
        // Create a new board
        Board board = new()
        {
            Game = Game,
            Width = 8,
            Height = 8,
            PlayerRows = 2,
            EnemyRows = 2,
            PlayerPieces = new List<Piece>(),
            EnemyPieces = new List<Piece>(),
            CornerBL = new Vector2(-4.0f, -1.0f)
        };
        board.CornerTR = board.CornerBL + new Vector2(board.Width, board.Height);

        // Create a square array of spaces
        board.Spaces = new Space[board.Width,board.Height];
        for (int x = 0; x < board.Width; x++)
        for (int y = 0; y < board.Height; y++)
        {
            board.Spaces[x,y] = new Space(board, x, y);
        }

        // Remove spaces in the desired locations
        int r = 0;
        for (int y = 0; y < board.Height / 2; y++)
        {
            for (int x = 0; x < r; x++)
            {
                board.Spaces[x,y] = null;
                board.Spaces[board.Width-1-x,y] = null;
            }
            r++;
        }
        r--;
        for (int y = board.Height / 2; y < board.Height; y++)
        {
            for (int x = 0; x < r; x++)
            {
                board.Spaces[x,y] = null;
                board.Spaces[board.Width-1-x,y] = null;
            }
            r--;
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
