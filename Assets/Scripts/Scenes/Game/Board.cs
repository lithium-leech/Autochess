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

    /// <summary>The number of rows (starting at the bottom) that the player can use</summary>
    public int PlayerRows { get; set; }
    
    /// <summary>The number of rows (starting at the top) that the enemy can use</summary>
    public int EnemyRows { get; set; }

    /// <summary>The pieces controlled by the player</summary>
    public List<Piece> PlayerPieces { get; }

    /// <summary>The pieces controlled by the enemy</summary>
    public List<Piece> EnemyPieces { get; }

    /// <summary>The world coordinates for this board's bottom left corner</summary>
    public Vector2 CornerBL { get; }

    /// <summary>The world coordinates for this board's top right corner</summary>
    public Vector2 CornerTR { get; }

    /// <summary>Creates a new instance of a Board</summary>
    /// <param name="game">The Game that this board exists in</param>
    /// <param name="width">The number of horizontal spaces on the board</param>
    /// <param name="height">The number of vertical spaces on the board</param>
    /// <param name="playerRows">The number of rows the player can use</param>
    /// <param name="enemyRows">The number of rows the enemy can use</param>
    /// <param name="cornerBL">World-space coordinates for the bottom left corner of the board</param>
    public Board(Game game, int width, int height, int playerRows, int enemyRows, Vector2 cornerBL)
    {
        Game = game;
        Width = width;
        Height = height;
        Spaces = new Piece[width,height];
        PlayerRows = playerRows;
        EnemyRows = enemyRows;
        PlayerPieces = new List<Piece>();
        EnemyPieces = new List<Piece>();
        CornerBL = cornerBL;
        CornerTR = cornerBL + new Vector2(width, height);
    }

    /// <summary>Adds a new piece to the board in the first empty space</summary>
    /// <param name="kind">The kind of piece to add</param>
    /// <param name="player">True if the piece is for the player</param>
    /// <param name="white">True if the piece is white</param>
    public void AddPiece(AssetGroup.Piece kind, bool player, bool white) => AddPiece(kind, player, white, GetFirstEmptySpace());

    /// <summary>Adds a new piece to the board</summary>
    /// <param name="kind">The kind of piece to add</param>
    /// <param name="player">True if the piece is for the player</param>
    /// <param name="white">True if the piece is white</param>
    /// <param name="space">The space to place the piece at</param>
    public void AddPiece(AssetGroup.Piece kind, bool player, bool white, Vector2Int space) => AddPiece(Game.CreatePiece(kind, player, white), space);

    /// <summary>Adds a piece to the board in the first empty space</summary>
    /// <param name="piece">The piece to add</param>
    public void AddPiece(Piece piece) => AddPiece(piece, GetFirstEmptySpace());

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
        if (piece.IsPlayer) PlayerPieces.Add(piece);
        else EnemyPieces.Add(piece);

        // Warp the piece to its new location
        piece.WarpTo(space);
    }

    /// <summary>Removes a piece from this board</summary>
    /// <param name="piece">The piece to remove</param>
    public void RemovePiece(Piece piece)
    {
        if (Spaces[piece.Space.x, piece.Space.y] == piece) Spaces[piece.Space.x, piece.Space.y] = null;
        if (piece.IsPlayer) PlayerPieces.Remove(piece);
        else EnemyPieces.Remove(piece);
    }

    /// <summary>Deletes a piece from existence</summary>
    /// <param name="piece">The piece to capture</param>
    public void CapturePiece(Piece piece)
    {
        RemovePiece(piece);
        piece.IsCaptured = true;
    }

    /// <summary>Removes all pieces from the board</summary>
    public void Clear()
    {
        for (int x = 0; x < Width; x++)
        for (int y = 0; y < Height; y++)
            if (HasPiece(new Vector2Int(x, y))) CapturePiece(Spaces[x, y]);
    }

    /// <summary>Gets the first empty space on the board</summary>
    /// <returns>A board space</returns>
    public Vector2Int GetFirstEmptySpace()
    {
        Vector2Int space = new Vector2Int(0, 0);
        for (int j = Height - 1; j >= 0; j--)
        for (int i = 0; i < Width; i++)
        {
            space = new Vector2Int(i, j);
            if (!HasPiece(space)) return space;
        }
        return space;
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
        else if (Spaces[space.x, space.y].IsPlayer == isPlayerPiece) return false;
        else return true;
    }

    /// <summary>Checks if a space is usable by the player</summary>
    /// <param name="space">The space to check</param>
    /// <returns>True if the space is usable by the player</returns>
    public bool IsPlayerSpace(Vector2Int space)
    {
        if (OnBoard(space))
            return space.y < PlayerRows;
        return false;
    }

    /// <summary>Gets a board space from a given world position</summary>
    /// <param name="position">The world position to get a space for</param>
    /// <returns>A board space</returns>
    public Vector2Int ToSpace(Vector3 position) => new Vector2Int((int)Math.Floor(position.x - CornerBL.x), (int)Math.Floor(position.y - CornerBL.y));

    /// <summary>Gets the world coordinates for a given space</summary>
    /// <param name="space">The space to get coordinated for</param>
    /// <returns>World-space coordinates</returns>
    public Vector3 ToPosition(Vector2Int space) => new Vector3(CornerBL.x + space.x + 0.5f, CornerBL.y + space.y + 0.5f, GameState.PieceZ);
}
