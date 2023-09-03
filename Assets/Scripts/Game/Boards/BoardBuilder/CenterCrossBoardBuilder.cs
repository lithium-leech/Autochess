using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes a 7x7 board with a cross in the center
/// </summary>
public class CenterCrossBoardBuilder : BoardBuilder
{
    /// <summary>Creates a new instance of a CenterCrossBoardBuilder</summary>
    /// <param name="game">The game to build a board in</param>
    public CenterCrossBoardBuilder(Game game) => Game = game;

    public override Board Build()
    {
        // Create a new board
        Board board = new()
        {
            Game = Game,
            Width = 7,
            Height = 7,
            PlayerRows = 2,
            EnemyRows = 2,
            PlayerPieces = new List<Piece>(),
            EnemyPieces = new List<Piece>(),
            CornerBL = new Vector2(-3.5f, -0.5f)
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
        board.Spaces[3,3] = null;
        board.Spaces[2,3] = null;
        board.Spaces[4,3] = null;
        board.Spaces[3,2] = null;
        board.Spaces[3,4] = null;

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
