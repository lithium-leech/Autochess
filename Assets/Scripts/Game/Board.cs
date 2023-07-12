using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A game board that pieces can move on
/// </summary>
public class Board
{
    /// <summary>The Game that this board exists in</summary>
    public Game Game { get; }
    
    /// <summary>The number of spaces going horizontally</summary>
    public int Width { get; }

    /// <summary>The number of spaces going vertically</summary>
    public int Height { get; }

    /// <summary>The board spaces and the pieces occupying them</summary>
    public Space[,] Spaces { get; }

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
        Spaces = new Space[width,height];
        for (int x = 0; x < Width; x++)
        for (int y = 0; y < Height; y++)
            Spaces[x, y] = new Space(this, x, y);
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
    public bool AddPiece(AssetGroup.Piece kind, bool player, bool white) => AddPiece(kind, player, white, GetFirstEmptySpace());

    /// <summary>Adds a new piece to the board</summary>
    /// <param name="kind">The kind of piece to add</param>
    /// <param name="player">True if the piece is for the player</param>
    /// <param name="white">True if the piece is white</param>
    /// <param name="space">The space to place the piece at</param>
    public bool AddPiece(AssetGroup.Piece kind, bool player, bool white, Vector2Int space) => AddPiece(Game.CreatePiece(kind, player, white), space);

    /// <summary>Adds a piece to the board in the first empty space</summary>
    /// <param name="piece">The piece to add</param>
    public bool AddPiece(Piece piece) => AddPiece(piece, GetFirstEmptySpace());

    /// <summary>Adds a piece to the board</summary>
    /// <param name="piece">The piece to add</param>
    /// <param name="space">The space to place the piece at</param>
    public bool AddPiece(Piece piece, Vector2Int space) => AddObject(piece, space);

    /// <summary>Adds a new object to the board in the last empty space</summary>
    /// <param name="kind">The kind of object to add</param>
    /// <param name="player">True if the object is for the player</param>
    /// <param name="white">True if the object is white</param>
    public bool AddObject(AssetGroup.Object kind, bool player, bool white) => AddObject(kind, player, white, GetLastEmptySpace());

    /// <summary>Adds a new object to the board</summary>
    /// <param name="kind">The kind of object to add</param>
    /// <param name="player">True if the object is for the player</param>
    /// <param name="white">True if the object is white</param>
    /// <param name="space">The space to place the object at</param>
    public bool AddObject(AssetGroup.Object kind, bool player, bool white, Vector2Int space) => AddObject(Game.CreateObject(kind, player, white), space);

    /// <summary>Adds an object to the board in the last empty space</summary>
    /// <param name="obj">The object to add</param>
    public bool AddObject(ChessObject obj) => AddObject(obj, GetLastEmptySpace());

    /// <summary>Adds an object to the board</summary>
    /// <param name="obj">The object to add</param>
    /// <param name="space">The space to place the object at</param>
    public bool AddObject(ChessObject obj, Vector2Int space) => Spaces[space.x,space.y].AddObject(obj);

    /// <summary>Destroys all objects on the board</summary>
    public void Clear()
    {
        for (int x = 0; x < Width; x++)
        for (int y = 0; y < Height; y++)
            Spaces[x,y].Clear();
    }

    /// <summary>Gets the first empty space on the board</summary>
    /// <returns>A board space</returns>
    /// <remarks>The top-left is "first" and right of that is "second"</remarks>
    /// <remarks>Returns (-1, -1) if there is no empty space</remarks>
    public Vector2Int GetFirstEmptySpace()
    {
        Vector2Int space = new Vector2Int(-1, -1);
        for (int j = Height - 1; j >= 0; j--)
        for (int i = 0; i < Width; i++)
            if (Spaces[i,j].IsEmpty()) return new Vector2Int(i, j);
        return space;
    }

    /// <summary>Gets the last empty space on the board</summary>
    /// <returns>A board space</returns>
    /// <remarks>The bottom-right is "last" and left of that is "second last"</remarks>
    /// <remarks>Returns (-1, -1) if there is no empty space</remarks>
    public Vector2Int GetLastEmptySpace()
    {
        Vector2Int space = new Vector2Int(-1, -1);
        for (int j = 0; j < Height; j++)
        for (int i = Width - 1; i >= 0; i--)
            if (Spaces[i,j].IsEmpty()) return new Vector2Int(i, j);
        return space;
    }

    /// <summary>Checks if a space is on the board</summary>
    /// <param name="space">The space to check</param>
    /// <returns>True if the space is on the board</returns>
    public bool OnBoard(Vector2Int space) => space.x >= 0 && space.x < Width && space.y >= 0 && space.y < Height;

    /// <summary>Gets a space on the board</summary>
    /// <param name="space">The space to get</param>
    /// <returns>The desired space, or null if the target space was invalid</returns>
    public Space GetSpace(Vector2Int space)
    {
        if (OnBoard(space)) return Spaces[space.x,space.y];
        else return null;
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
