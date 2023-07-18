using UnityEngine;

/// <summary>
/// A class containing Game extensions
/// </summary>
public static class GameExtensions
{
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
        piece.IsPlayer = player;
        piece.IsWhite = white;
        piece.IsGrabable = player;
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
        chessObject.IsPlayer = player;
        chessObject.IsWhite = white;
        chessObject.IsGrabable = player;
        return chessObject;
    }

    /// <summary>Creates a new instance of a power</summary>
    /// <param name="game">The game to create a power in</param>
    /// <param name="kind">The kind of power to create</param>
    /// <param name="player">True if the power is for the player</param>
    /// <param name="remove">True if this is a power remover</param>
    /// <returns>A Power</returns>
    public static Power CreatePower(this Game game, AssetGroup.Power kind, bool player, bool remove)
    {
        AssetGroup.Group groupKey = remove ? AssetGroup.Group.RemovePower : AssetGroup.Group.Power;
        GameObject obj = Game.Instantiate(AssetManager.Prefabs[groupKey][(int)kind]);
        Power power = obj.GetComponent<Power>();
        power.Game = game;
        power.IsPlayer = player;
        return power;
    }

    /// <summary>Creates a new instance of a panel</summary>
    /// <param name="game">The game to create a panel in</param>
    /// <param name="white">True if the panel is white</param>
    /// <param name="info">True if the panel is an info panel</param>
    /// <returns>A Panel</returns>
    public static GameObject CreatePanel(this Game game, bool white, bool info)
    {
        AssetGroup.Panel kind;
        if (white)
            if (info) kind = AssetGroup.Panel.WhiteInfo;
            else kind = AssetGroup.Panel.WhiteTile;
        else
            if (info) kind = AssetGroup.Panel.BlackInfo;
            else kind = AssetGroup.Panel.BlackTile;
        GameObject panel = Game.Instantiate(AssetManager.Prefabs[AssetGroup.Group.Panel][(int)kind]);
        return panel;
    }

    /// <summary>Creates a new instance of a single highlight object</summary>
    /// <param name="game">The game to create a highlight in</param>
    /// <param name="kind">The kind of highlight to create</param>
    /// <param name="green">True if the highlight is green</param>
    /// <param name="board">The board to load the highlight on</param>
    /// <param name="space">The space to place the highlight on</param>
    /// <returns>A new highlight object</returns>
    public static GameObject CreateHighlight(this Game game, AssetGroup.Highlight kind, bool green, Board board, Vector2Int space)
    {
        AssetGroup.Group groupKey = green ? AssetGroup.Group.GreenHighlight : AssetGroup.Group.RedHighlight;
        GameObject highlight = Game.Instantiate(AssetManager.Prefabs[groupKey][(int)kind]);
        highlight.transform.position = board.ToPosition(space) + GameState.PieceZBottom;
        return highlight;
    }
}
