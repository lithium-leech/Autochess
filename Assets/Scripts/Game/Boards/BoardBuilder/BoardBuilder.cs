using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An object for generating a particular board configuration
/// </summary>
public abstract class BoardBuilder
{
    /// <summary>Builds new game boards</summary>
    /// <returns>A new Board</returns>
    public abstract Board Build();

    /// <summary>The game to make the board in</summary>
    protected Game Game;

    /// <summary>Creates tiles for the given board</summary>
    /// <param name="board">The board to create tiles for</param>
    /// <param name="back">True if these tiles are in the backdrop</param>
    protected void CreateTiles(Board board, bool back)
    {
        // Delete any existing tiles
        if (board.Tiles != null)
            foreach (GameObject tile in board.Tiles)
                Game.Destroy(tile);

        // Create a new collection of tiles
        board.Tiles = new List<GameObject>();
        
        // Iterate the spaces plus their perimeter
        for (int x = -1; x <= board.Width; x++)
        for (int y = -1; y <= board.Height; y++)
        {
            // Get the space and position of the current coordinates
            Vector2Int coordinates = new(x, y);
            Space space = board.GetSpace(coordinates);
            Vector3 position = board.ToPosition(coordinates);
            position += back ? GameState.BackboardZ : GameState.BoardZ;

            // Create a tile for the space
            GameObject tile;
            if (space == null) tile = CreateBorderTile(board, coordinates, position);
            else tile = CreateSpaceTile(board, coordinates, position);
            if (tile != null) board.Tiles.Add(tile);
        }
    }

    /// <summary>Creates a single tile for a space on the board</summary>
    /// <param name="board">The board to create the tile for</param>
    /// <param name="coordinates">The coordinates of the space to create a tile for</param>
    /// <param name="position">The world position to create the tile at</param>
    /// <returns>A newly create tile</returns>
    private GameObject CreateSpaceTile(Board board, Vector2Int coordinates, Vector3 position)
    {
        bool white = ((coordinates.x % 2) + (coordinates.y % 2)) % 2 == 1;
        if (white) return Game.CreateTile(AssetGroup.Tile.WhiteSpace, position);
        else return Game.CreateTile(AssetGroup.Tile.BlackSpace, position);
    }

    /// <summary>Creates a single tile for a space off the board</summary>
    /// <param name="board">The board to create the tile for</param>
    /// <param name="coordinates">The coordinates of the space to create a tile for</param>
    /// <param name="position">The world position to create the tile at</param>
    /// <returns>A newly create tile</returns>
    private GameObject CreateBorderTile(Board board, Vector2Int coordinates, Vector3 position)
    {
        // Get the space above and below
        Space above = board.GetSpace(coordinates + new Vector2Int(0, 1));
        Space below = board.GetSpace(coordinates + new Vector2Int(0, -1));

        // Check if they are promotion spaces
        bool topPromotion = above != null && above.IsEnemyPromotion;
        bool bottomPromotion = below != null && below.IsPlayerPromotion;

        // Determine which neighboring spaces are present
        bool top = above != null;
        bool bottom = below != null;
        bool right = board.GetSpace(coordinates + new Vector2Int(1, 0)) != null;
        bool left = board.GetSpace(coordinates + new Vector2Int(-1, 0)) != null;
        bool topRight = board.GetSpace(coordinates + new Vector2Int(1, 1)) != null;
        bool topLeft = board.GetSpace(coordinates + new Vector2Int(-1, 1)) != null;
        bool bottomRight = board.GetSpace(coordinates + new Vector2Int(1, -1)) != null;
        bool bottomLeft = board.GetSpace(coordinates + new Vector2Int(-1, -1)) != null;

        // Create convenient keys based on the discovered information
        int sKey = (bottomPromotion ? 1 : 0) << 0 |
                   (topPromotion ? 1 : 0) << 1 |
                   (bottom ? 1 : 0) << 2 |
                   (top ? 1 : 0) << 3 |
                   (left ? 1 : 0) << 4 |
                   (right ? 1 : 0) << 5;
        int cKey = (bottomLeft ? 1 : 0) << 0 |
                   (bottomRight ? 1 : 0) << 1 |
                   (topLeft ? 1 : 0) << 2 |
                   (topRight ? 1 : 0) << 3;

        // Get the kind of tile needed
        AssetGroup.Tile kind = DetermineTileKind(sKey, cKey);

        // Return null if this is not a tile
        if (kind == AssetGroup.Tile.None) return null;

        // Create the tile
        return Game.CreateTile(kind, position);
    }

    /// <summary>Determines the kind of tile that matches the given information</summary>
    /// <param name="sKey">A bit key containing information about the space's neighboring sides</param>
    /// <param name="cKey">A bit key containing information about the space's neighboring corners</param>
    /// <returns>A Tile kind</returns>
    private AssetGroup.Tile DetermineTileKind(int sKey, int cKey)
    {
        // Check the bordering sides
        AssetGroup.Tile kind = sKey switch
        {
            4 => AssetGroup.Tile.BorderT,
            5 => AssetGroup.Tile.BorderPT,
            8 => AssetGroup.Tile.BorderB,
            10 => AssetGroup.Tile.BorderPB,
            12 => AssetGroup.Tile.InsideBorderTB,
            13 => AssetGroup.Tile.InsideBorderPTB,
            14 => AssetGroup.Tile.InsideBorderTPB,
            15 => AssetGroup.Tile.InsideBorderPTPB,
            16 => AssetGroup.Tile.BorderR,
            20 => AssetGroup.Tile.InsideBorderTR,
            21 => AssetGroup.Tile.InsideBorderPTR,
            24 => AssetGroup.Tile.InsideBorderBR,
            26 => AssetGroup.Tile.InsideBorderPBR,
            28 => AssetGroup.Tile.InsideBorderTBR,
            29 => AssetGroup.Tile.InsideBorderPTBR,
            30 => AssetGroup.Tile.InsideBorderTPBR,
            31 => AssetGroup.Tile.InsideBorderPTPBR,
            32 => AssetGroup.Tile.BorderL,
            36 => AssetGroup.Tile.InsideBorderTL,
            37 => AssetGroup.Tile.InsideBorderPTL,
            40 => AssetGroup.Tile.InsideBorderBL,
            42 => AssetGroup.Tile.InsideBorderPBL,
            44 => AssetGroup.Tile.InsideBorderTBL,
            45 => AssetGroup.Tile.InsideBorderPTBL,
            46 => AssetGroup.Tile.InsideBorderTPBL,
            47 => AssetGroup.Tile.InsideBorderPTPBL,
            48 => AssetGroup.Tile.InsideBorderRL,
            52 => AssetGroup.Tile.InsideBorderTRL,
            53 => AssetGroup.Tile.InsideBorderPTRL,
            56 => AssetGroup.Tile.InsideBorderBRL,
            58 => AssetGroup.Tile.InsideBorderPBRL,
            60 => AssetGroup.Tile.InsideBorderTBRL,
            61 => AssetGroup.Tile.InsideBorderPTBRL,
            62 => AssetGroup.Tile.InsideBorderTPBRL,
            63 => AssetGroup.Tile.InsideBorderPTPBRL,
            _ => AssetGroup.Tile.None
        };
        if (kind != AssetGroup.Tile.None) return kind;

        // Check the bordering corners
        return cKey switch
        {
            1 => AssetGroup.Tile.BorderTR,
            2 => AssetGroup.Tile.BorderTL,
            3 => AssetGroup.Tile.BorderTRTL,
            4 => AssetGroup.Tile.BorderBR,
            5 => AssetGroup.Tile.BorderTRBR,
            6 => AssetGroup.Tile.BorderTLBR,
            7 => AssetGroup.Tile.BorderTRTLBR,
            8 => AssetGroup.Tile.BorderBL,
            9 => AssetGroup.Tile.BorderTRBL,
            10 => AssetGroup.Tile.BorderTLBL,
            11 => AssetGroup.Tile.BorderTRTLBL,
            12 => AssetGroup.Tile.BorderBRBL,
            13 => AssetGroup.Tile.BorderTRBRBL,
            14 => AssetGroup.Tile.BorderTLBRBL,
            15 => AssetGroup.Tile.BorderTRTLBRBL,
            _ => AssetGroup.Tile.None,
        };
    }
}
