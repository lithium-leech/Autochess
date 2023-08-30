using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes a small 4x1 board for displaying powers above or below the game board
/// /// </summary>
public class PowerBoardBuilder : BoardBuilder
{
    /// <summary>Creates a new instance of a PowerBoardBuilder</summary>
    /// <param name="game">The game to build a board in</param>
    /// <param name="player">True if this is the player's power board</param>
    public PowerBoardBuilder(Game game, bool player)
    {
        Game = game;
        IsPlayer = player;
    }

    /// <summary>True if this is the player's power board</summary>
    private bool IsPlayer { get; }

    public override Board Build()
    {
        // Create a new board
        Board board = new()
        {
            Game = Game,
            Width = 4,
            Height = 1,
            PlayerRows = 1,
            EnemyRows = 0,
            PlayerPieces = new List<Piece>(),
            EnemyPieces = new List<Piece>(),
            CornerBL = IsPlayer ? new Vector2(-2.0f, -2.5f) : new Vector2(-2.0f, 7.5f),
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
