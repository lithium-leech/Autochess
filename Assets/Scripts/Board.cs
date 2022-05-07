using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A game board that pieces can move on
/// </summary>
public class Board
{
    /// <summary>The Game that this board exists in</summary>
    private Game Game { get; }
    
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
    public float XWorld { get; }

    /// <summary>The bottom side of the board's y coordinate in the world-space</summary>
    public float YWorld { get; }

    /// <summary>Creates a new instance of a Board</summary>
    /// <param name="game">The Game that this board exists in</param>
    /// <param name="width">The number of horizontal spaces on the board</param>
    /// <param name="height">The number of vertical spaces on the board</param>
    /// <param name="x">A world-space x-coordinate to center the board on</param>
    /// <param name="y">A world-space y-coordinate to center the board on</param>
    public Board(Game game, int width, int height, float x, float y)
    {
        Game = game;
        Width = width;
        Height = height;
        Spaces = new Piece[width,height];
        PlayerPieces = new List<Piece>();
        EnemyPieces = new List<Piece>();
        XWorld = x;
        YWorld = y;
    }

    /// <summary>Adds a new piece to the board</summary>
    /// <typeparam name="T">The type of piece to add</typeparam>
    /// <param name="white">True if the piece is white</param>
    /// <param name="space">The space to place the piece at</param>
    public void AddPiece<T>(bool white, Vector2Int space) where T : Piece => AddPiece(typeof(T), white, space);

    /// <summary>Adds a new piece to the board</summary>
    /// <param name="type">The type of piece to add</param>
    /// <param name="white">True if the piece is white</param>
    /// <param name="space">The space to place the piece at</param>
    public void AddPiece(Type type, bool white, Vector2Int space) => AddPiece(Game.CreatePiece(type, white), space);

    /// <summary>Adds a piece to the board</summary>
    /// <param name="piece">The piece to add</param>
    /// <param name="space">The space to place the piece at</param>
    public void AddPiece(Piece piece, Vector2Int space)
    {
        // Ensure that the space is on the board
        if (space.x < 0) space.x = 0;
        if (space.x >= Width) space.x = Width - 1;
        if (space.y < 0) space.y = 0;
        if (space.y >= Height) space.y = Height - 1;
        
        // Update the piece
        piece.Board = this;
        piece.Space = space;

        // Remove any piece on the specified space (if present)
        Piece existing = Spaces[space.x,space.y];
        if (existing != null)
        {
            Spaces[space.x,space.y] = null;
            PlayerPieces.Remove(existing);
            EnemyPieces.Remove(existing);
            CapturePiece(existing);
        }

        // Add the piece to the appropriate collections
        Spaces[space.x,space.y] = piece;
        if (piece.IsPlayerPiece) PlayerPieces.Add(piece);
        else EnemyPieces.Add(piece);

        // Warp the piece to its new location
        piece.WarpTo(space);
    }

    /// <summary>Removes a piece from this board</summary>
    /// <param name="piece">The piece to remove</param>
    public void RemovePiece(Piece piece)
    {
        if (Spaces[piece.Space.x, piece.Space.y] == piece) Spaces[piece.Space.x, piece.Space.y] = null;
        if (piece.IsPlayerPiece) PlayerPieces.Remove(piece);
        else EnemyPieces.Remove(piece);
    }

    /// <summary>Deletes a piece from existence</summary>
    /// <param name="piece">The piece to capture</param>
    public void CapturePiece(Piece piece)
    {
        RemovePiece(piece);
        piece.IsCaptured = true;
    }

    /// <summary>Checks if a space is on the board</summary>
    /// <param name="space">The space to check</param>
    /// <returns>True if the space is on the board</returns>
    public bool OnBoard(Vector2Int space) => space.x >= 0 && space.x < Width && space.y >= 0 && space.y < Height;

    /// <summary>Checks if a space has a piece on it</summary>
    /// <param name="space">The space to check</param>
    /// <returns>True if the space has a piece on it</returns>
    public bool HasPiece(Vector2Int space)
    {
        if (OnBoard(space))
            if (Spaces[space.x, space.y] != null)
                return true;
        return false;
    }

    /// <summary>Checks if a space has an enemy piece on it</summary>
    /// <param name="isPlayerPiece">True if non-player pieces are enemies</param>
    /// <param name="space">The space to check</param>
    /// <returns>True if the space has an enemy piece on it</returns>
    public bool HasEnemy(bool isPlayerPiece, Vector2Int space)
    {
        if (!HasPiece(space)) return false;
        else if (Spaces[space.x, space.y].IsPlayerPiece == isPlayerPiece) return false;
        else return true;
    }
}
