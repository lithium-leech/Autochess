using UnityEngine;

/// <summary>
/// A class containing Game extensions
/// </summary>
public static class GameExtensions
{
    /// <summary>Creates a new instance of a tile</summary>
    /// <param name="game">The game to create a tile in</param>
    /// <param name="kind">The kind of tile to create</param>
    /// <param name="position">The position to create the tile at</param>
    /// <returns>A Tile</returns>
    public static GameObject CreateTile(this Game game, AssetGroup.Tile kind, Vector3 position)
    {
        GameObject tile = Game.Instantiate(AssetManager.Prefabs[AssetGroup.Group.Tile][(int)kind]);
        tile.transform.position = position;
        return tile;
    }

    /// <summary>Creates a new instance of a panel</summary>
    /// <param name="game">The game to create a panel in</param>
    /// <param name="kind">The kind of panel to create</param>
    /// <param name="position">The position to create the panel at</param>
    /// <returns>A Panel</returns>
    public static GameObject CreatePanel(this Game game, AssetGroup.Panel kind, Vector3 position, Transform parent)
    {
        GameObject panel = Game.Instantiate(AssetManager.Prefabs[AssetGroup.Group.Panel][(int)kind]);
        panel.transform.position = position;
        if (parent != null)
        {
            panel.transform.SetParent(parent);
            panel.transform.SetAsFirstSibling();
        }
        return panel;
    }

    /// <summary>Creates a new instance of a single highlight object</summary>
    /// <param name="game">The game to create a highlight in</param>
    /// <param name="kind">The kind of highlight to create</param>
    /// <param name="green">True if the highlight is green</param>
    /// <param name="board">The board to load the highlight on</param>
    /// <param name="coordinates">The coordinates to place the highlight at</param>
    /// <returns>A new highlight object</returns>
    public static GameObject CreateHighlight(this Game game, AssetGroup.Highlight kind, bool green, Board board, Vector2Int coordinates)
    {
        AssetGroup.Group groupKey = green ? AssetGroup.Group.GreenHighlight : AssetGroup.Group.RedHighlight;
        GameObject highlight = Game.Instantiate(AssetManager.Prefabs[groupKey][(int)kind]);
        highlight.transform.position = board.ToPosition(coordinates) + GameState.HighlightZ;
        return highlight;
    }

    /// <summary>Creates a new instance of a piece</summary>
    /// <param name="game">The game to create a piece in</param>
    /// <param name="kind">The kind of piece to create</param>
    /// <param name="player">True if the piece is for the player</param>
    /// <param name="white">True if the piece is white</param>
    /// <returns>A Piece</returns>
    public static Piece CreatePiece(this Game game, AssetGroup.Piece kind, bool player, bool white)
    {
        AssetGroup.Group groupKey = white ? AssetGroup.Group.WhitePiece : AssetGroup.Group.BlackPiece;
        GameObject obj = Game.Instantiate(AssetManager.Prefabs[groupKey][(int)kind]);
        Piece piece = obj.GetComponent<Piece>();
        piece.Initialize(game, player, white);
        return piece;
    }

    /// <summary>Creates a new instance of a chess object</summary>
    /// <param name="game">The game to create a chess object in</param>
    /// <param name="kind">The kind of object to create</param>
    /// <param name="player">True if the object is for the player</param>
    /// <param name="white">True if the object is white</param>
    /// <returns>A Piece</returns>
    public static ChessObject CreateObject(this Game game, AssetGroup.Object kind, bool player, bool white)
    {
        AssetGroup.Group groupKey = white ? AssetGroup.Group.WhiteObject : AssetGroup.Group.BlackObject;
        GameObject obj = Game.Instantiate(AssetManager.Prefabs[groupKey][(int)kind]);
        ChessObject chessObject = obj.GetComponent<ChessObject>();
        chessObject.Initialize(game, player, white);
        return chessObject;
    }

    /// <summary>Creates a new instance of a power</summary>
    /// <param name="game">The game to create a power in</param>
    /// <param name="kind">The kind of power to create</param>
    /// <param name="player">True if the power is for the player</param>
    /// <returns>A Power</returns>
    public static Power CreatePower(this Game game, AssetGroup.Power kind, bool player)
    {
        GameObject obj = Game.Instantiate(AssetManager.Prefabs[AssetGroup.Group.Power][(int)kind]);
        Power power = obj.GetComponent<Power>();
        power.Initialize(game, player, true);
        return power;
    }
}
