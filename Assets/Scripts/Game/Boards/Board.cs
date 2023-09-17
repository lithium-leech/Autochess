using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A game board that pieces can move on
/// </summary>
public class Board
{
    /// <summary>The Game that this board exists in</summary>
    public Game Game { get; set; }
    
    /// <summary>The number of spaces going horizontally</summary>
    public int Width { get; set; }

    /// <summary>The number of spaces going vertically</summary>
    public int Height { get; set; }

    /// <summary>The board spaces and the pieces occupying them</summary>
    public Space[,] Spaces { get; set; }

    /// <summary>The number of rows (starting at the bottom) that the player can use</summary>
    public int PlayerRows { get; set; }
    
    /// <summary>The number of rows (starting at the top) that the enemy can use</summary>
    public int EnemyRows { get; set; }

    /// <summary>The pieces controlled by the player</summary>
    public List<Piece> PlayerPieces { get; set; }

    /// <summary>The pieces controlled by the enemy</summary>
    public List<Piece> EnemyPieces { get; set; }

    /// <summary>The world coordinates for this board's bottom left corner</summary>
    public Vector2 CornerBL { get; set; }

    /// <summary>The world coordinates for this board's top right corner</summary>
    public Vector2 CornerTR { get; set; }

    /// <summary>The tile sprites that make up the visible parts of the board</summary>
    public IList<GameObject> Tiles { get; set; }

    /// <summary>Creates a new instance of a Board</summary>
    public Board() { }

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
    public bool AddPiece(AssetGroup.Piece kind, bool player, bool white, Space space)
    {
        // Create the new piece
        Piece piece = Game.CreatePiece(kind, player, white);

        // Flip pieces with directional sprites
        switch (kind)
        {
            case AssetGroup.Piece.Fuhyo:
            case AssetGroup.Piece.Ginsho:
            case AssetGroup.Piece.Hisha:
            case AssetGroup.Piece.Kakugyo:
            case AssetGroup.Piece.Keima:
            case AssetGroup.Piece.Kinsho:
            case AssetGroup.Piece.Kyosha:
            case AssetGroup.Piece.Narigin:
            case AssetGroup.Piece.Narikei:
            case AssetGroup.Piece.Narikyo:
            case AssetGroup.Piece.Osho:
            case AssetGroup.Piece.Ryuma:
            case AssetGroup.Piece.Ryuo:
            case AssetGroup.Piece.Tokin:
            case AssetGroup.Piece.Zu:
            case AssetGroup.Piece.Ju:
            case AssetGroup.Piece.Ma:
            case AssetGroup.Piece.Xiang:
            case AssetGroup.Piece.Jiang:
            case AssetGroup.Piece.Shi:
            case AssetGroup.Piece.Pao:
            if (!player) piece.transform.localScale = new Vector3(piece.transform.localScale.x, -1f, piece.transform.localScale.z);
            break;
            default:
            // No action required
            break;
        }

        // Add the new piece to the board
        return  AddPiece(piece, space);
    }

    /// <summary>Adds a piece to the board in the first empty space</summary>
    /// <param name="piece">The piece to add</param>
    public bool AddPiece(Piece piece) => AddPiece(piece, GetFirstEmptySpace());

    /// <summary>Adds a piece to the board</summary>
    /// <param name="piece">The piece to add</param>
    /// <param name="space">The space to place the piece at</param>
    public bool AddPiece(Piece piece, Space space) => AddObject(piece, space);

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
    public bool AddObject(AssetGroup.Object kind, bool player, bool white, Space space) => AddObject(Game.CreateObject(kind, player, white), space);

    /// <summary>Adds an object to the board in the last empty space</summary>
    /// <param name="obj">The object to add</param>
    public bool AddObject(ChessObject obj) => AddObject(obj, GetLastEmptySpace());

    /// <summary>Adds a power to the board in the first empty space</summary>
    /// <param name="power">The power to add</param>
    public bool AddPower(Power power) => AddObject(power, GetFirstEmptySpace());

    /// <summary>Adds an object to the board</summary>
    /// <param name="obj">The object to add</param>
    /// <param name="space">The space to place the object at</param>
    public bool AddObject(ChessObject obj, Space space)
    {
        if (space == null) return false;
        else return space.AddObject(obj);
    }

    /// <summary>Destroys all objects on the board</summary>
    public void Clear()
    {
        for (int x = 0; x < Width; x++)
        for (int y = 0; y < Height; y++)
            GetSpace(new Vector2Int(x, y))?.Clear();
    }

    /// <summary>Checks if there are any moving pieces on the board</summary>
    /// <returns>True if any pieces on the board are actively moving</returns>
    public bool ArePiecesMoving()
    {
        foreach (Piece piece in PlayerPieces) if (piece.IsMoving) return true;
        foreach (Piece piece in EnemyPieces) if (piece.IsMoving) return true;
        return false;
    }

    /// <summary>Gets the first empty space on the board</summary>
    /// <returns>A board space</returns>
    /// <remarks>The top-left is "first" and right of that is "second"</remarks>
    /// <remarks>Returns null if there is no empty space</remarks>
    public Space GetFirstEmptySpace()
    {
        for (int j = Height - 1; j >= 0; j--)
        for (int i = 0; i < Width; i++)
            if (Spaces[i,j].IsEmpty()) return Spaces[i,j];
        return null;
    }

    /// <summary>Gets the last empty space on the board</summary>
    /// <returns>A board space</returns>
    /// <remarks>The bottom-right is "last" and left of that is "second last"</remarks>
    /// <remarks>Returns null if there is no empty space</remarks>
    public Space GetLastEmptySpace()
    {
        for (int j = 0; j < Height; j++)
        for (int i = Width - 1; i >= 0; i--)
            if (Spaces[i,j].IsEmpty()) return Spaces[i,j];
        return null;
    }

    /// <summary>Checks if a set of coordinates is on the board</summary>
    /// <param name="coordinates">The coordinates to check</param>
    /// <returns>True if the coordinates are on the board</returns>
    public bool OnBoard(Vector2Int coordinates) => coordinates.x >= 0 && coordinates.x < Width && coordinates.y >= 0 && coordinates.y < Height;

    /// <summary>Gets a space on the board</summary>
    /// <param name="coordinates">The desired board space's coordinates</param>
    /// <returns>The desired space, or null if the coordinates are invalid</returns>
    public Space GetSpace(Vector2Int coordinates)
    {
        if (OnBoard(coordinates)) return Spaces[coordinates.x,coordinates.y];
        else return null;
    }

    /// <summary>Gets a board space's coordinates from a given world position</summary>
    /// <param name="position">The world position to get coordinates for</param>
    /// <returns>A board space's coordinates</returns>
    public Vector2Int ToSpace(Vector3 position) => new((int)Math.Floor(position.x - CornerBL.x), (int)Math.Floor(position.y - CornerBL.y));

    /// <summary>Gets the world position for a given board space's coordinates</summary>
    /// <param name="coordinates">The board space's coordinates to get a position for</param>
    /// <returns>A world position (z is 0)</returns>
    public Vector3 ToPosition(Vector2Int coordinates) => new(CornerBL.x + coordinates.x + 0.5f, CornerBL.y + coordinates.y + 0.5f, 0.0f);

    /// <summary>Destroys this game board</summary>
    public void Destroy()
    {
        Clear();
        foreach (GameObject tile in Tiles) Game.Destroy(tile);
    }
}
