using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Board
{
    public int Width { get; } // The number of spaces going horizontally
    public int Height { get; } // The number of spaces going vertically
    public Piece[,] Spaces { get; } // The board spaces and the pieces occupying them
    public List<Piece> PlayerPieces { get; } // The pieces controlled by the player
    public List<Piece> EnemyPieces { get; } // The pieces controlled by the enemy
    private double XWorld { get; } // The board's x coordinate in the world space
    private double YWorld { get; } // The board's y coordinate in the world space
    
    /// <summary></summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public Board(int width, int height, double x, double y)
    {
        Width = width;
        Height = height;
        Spaces = new Piece[width,height];
        PlayerPieces = new List<Piece>();
        EnemyPieces = new List<Piece>();
        XWorld = x;
        YWorld = y;
    }

    /// <summary></summary>
    /// <param name="piece"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="isPlayer"></param>
    public void Add(Piece piece, int x, int y, bool isPlayer)
    {
        // Update the piece
        piece.Board = this;
        piece.X = x;
        piece.Y = y;

        // Check that x and y are in bounds
        if (x < 1) x = 1;
        if (x > Width) x = Width;
        if (y < 1) y = 1;
        if (y > Height) y = Height;

        // Remove the piece on the specified space (if present)
        Piece existing = Spaces[x,y];
        if (existing != null)
        {
            Spaces[x,y] = null;
            PlayerPieces.Remove(existing);
            EnemyPieces.Remove(existing);
            existing.Destroy();
        }

        // Add the piece to the appropriate collections
        Spaces[x,y] = piece;
        if (isPlayer) PlayerPieces.Add(piece);
        else EnemyPieces.Add(piece); 
    }
}
