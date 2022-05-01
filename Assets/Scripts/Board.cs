using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>The left side of the board's x coordinate in the world-space</summary>
    private float XWorld { get; }

    /// <summary>The bottom side of the board's y coordinate in the world-space</summary>
    private float YWorld { get; }
    
    /// <summary>Creates a new instance of a Board</summary>
    /// <param name="width">The number of horizontal spaces on the board</param>
    /// <param name="height">The number of vertical spaces on the board</param>
    /// <param name="x">A world-space x-coordinate to center the board on</param>
    /// <param name="y">A world-space y-coordinate to center the board on</param>
    public Board(int width, int height, float x, float y)
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
    /// <param name="space">The space to place the piece at</param>
    public void Add(Piece piece, Vector2Int space)
    {
        // Ensure that the space is on the board
        if (space.x < 0) space.x = 0;
        if (space.x >= Width) space.x = Width - 1;
        if (space.y < 0) space.y = 0;
        if (space.y >= Height) space.y = Height - 1;
        
        // Update the piece
        piece.Board = this;
        piece.Space = space;

        // Remove the piece on the specified space (if present)
        Piece existing = Spaces[space.x,space.y];
        if (existing != null)
        {
            Spaces[space.x,space.y] = null;
            PlayerPieces.Remove(existing);
            EnemyPieces.Remove(existing);
            existing.Captured();
        }

        // Add the piece to the appropriate collections
        Spaces[space.x,space.y] = piece;
        if (piece.IsPlayerPiece) PlayerPieces.Add(piece);
        else EnemyPieces.Add(piece);

        // Warp the piece to its new location
        Vector2 target = GetWorldPosition(space);
        piece.Behavior.WarpTo(target);
    }

    public void Move(Piece piece, Vector2Int space)
    {
        // Check that this piece is where it's supposed to be
        if (Spaces[piece.Space.x, piece.Space.y] != piece)
        {
            // Destroy this piece if it is not
            piece.Captured();
            return;
        }

        // Do nothing if the piece is remaining stationary
        if (piece.Space == space) return;

        // Destroy the captured piece if there is one
        Piece captured = Spaces[space.x, space.y];
        if (captured != null) captured.Captured();

        // Update the piece's location
        Spaces[piece.Space.x, piece.Space.y] = null;
        Spaces[space.x, space.y] = piece;
        piece.Space = space;

        // Move the piece to its new location
        Vector2 target = GetWorldPosition(space);
        piece.Behavior.MoveTo(target);
    }

    /// <summary>Gets the world coordinates for a given space</summary>
    /// <param name="space">The space to get coordinated for</param>
    /// <returns>World-space coordinates</returns>
    private Vector2 GetWorldPosition(Vector2Int space) => new(XWorld + space.x + 0.5f, YWorld + space.y + 0.5f);
}
