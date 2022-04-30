using System.Collections.Generic;

/// <summary>
/// A game board that pieces can move on
/// </summary>
public class Board
{
    /// <summary>The number of spaces going horizontally</summary>
    public int Width { get; }

    /// <summary>The number of spaces going vertically</summary>
    public int Height { get; }

    /// <summary>The board spaces and the pieces occupying them</summary>
    public Piece[,] Spaces { get; }

    /// <summary>The pieces controlled by the player</summary>
    public List<Piece> PlayerPieces { get; }

    /// <summary>The pieces controlled by the enemy</summary>
    public List<Piece> EnemyPieces { get; }

    /// <summary>The board's x coordinate in the world-space</summary>
    private double XWorld { get; }

    /// <summary>The board's y coordinate in the world-space</summary>
    private double YWorld { get; }
    
    /// <summary>Creates a new instance of a Board</summary>
    /// <param name="width">The number of horizontal spaces on the board</param>
    /// <param name="height">The number of vertical spaces on the board</param>
    /// <param name="x">A world-space x-coordinate to center the board on</param>
    /// <param name="y">A world-space y-coordinate to center the board on</param>
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

    /// <summary>Adds a piece to the board</summary>
    /// <param name="piece">The piece to add</param>
    /// <param name="x">The x coordinate to place the piece</param>
    /// <param name="y">The y coordinate to place the piece</param>
    /// <param name="isPlayer">True if this piece is controlled by the player</param>
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
