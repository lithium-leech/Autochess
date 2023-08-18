using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class containing methods for the enemy's decision making during piece placement
/// </summary>
public static class PlacementAI
{
    /// <summary>Sets up the enemy's pieces on the game board</summary>
    /// <param name="game">The game to set up the enemy's pieces in</param>
    public static void SetUpEnemy(Game game)
    {
        foreach (AssetGroup.Piece pieceType in game.EnemyPieces) PlacePieceRandomly(game.GameBoard, pieceType);
        foreach (AssetGroup.Object objectType in game.EnemyObjects) PlaceObjectRandomly(game.GameBoard, objectType);   
    }

    /// <summary>Places a single piece</summary>
    /// <param name="board">The board to place the piece on</param>
    /// <param name="pieceType">The type of piece to place</param>
    private static void PlacePieceRandomly(Board board, AssetGroup.Piece pieceType)
    {
        board.AddPiece(pieceType, false, !GameState.IsPlayerWhite, RandomEnemyZone(board));
    }

    /// <summary>Places a single object</summary>
    /// <param name="board">The board to place the object on</param>
    /// <param name="objectType">The type of object to place</param>
    private static void PlaceObjectRandomly(Board board, AssetGroup.Object objectType)
    {
        Space space = objectType switch
        {
            AssetGroup.Object.Mine => RandomNeutralZone(board),
            AssetGroup.Object.Shield => RandomEnemyPiece(board),
            AssetGroup.Object.Wall => RandomNeutralZone(board),
            _ => null
        };
        if (space == null) return;
        board.AddObject(objectType, false, !GameState.IsPlayerWhite, space);
    }

    /// <summary>Gets a random empty space in the enemy zone</summary>
    /// <param name="board">The board to get a space from</param>
    /// <returns>An empty space in the enemy zone</returns>
    private static Space RandomEnemyZone(Board board)
    {
        IList<Space> emptySpaces = new List<Space>();
        for (int x = 0; x < board.Width; x++)
        for (int y = board.Height - board.EnemyRows; y < board.Height; y++)
        {
            Space space = board.GetSpace(new Vector2Int(x, y));
            if (space.IsEmpty()) emptySpaces.Add(space);
        }
        if (emptySpaces.Count < 1) return null;
        else return emptySpaces[UnityEngine.Random.Range(0, emptySpaces.Count - 1)];
    }

    /// <summary>Gets a random empty space in the neutral zone</summary>
    /// <param name="board">The board to get a space from</param>
    /// <returns>An empty space in the neutral zone</returns>
    private static Space RandomNeutralZone(Board board)
    {
        IList<Space> emptySpaces = new List<Space>();
        for (int x = 0; x < board.Width; x++)
        for (int y = board.PlayerRows; y < board.Height - board.EnemyRows; y++)
        {
            Space space = board.GetSpace(new Vector2Int(x, y));
            if (space.IsEmpty()) emptySpaces.Add(space);
        }
        if (emptySpaces.Count < 1) return null;
        else return emptySpaces[UnityEngine.Random.Range(0, emptySpaces.Count - 1)];
    }

    /// <summary>Gets a random space occupied with an enemy piece</summary>
    /// <param name="board">The board to get a space from</param>
    /// <returns>A space occupied with an enemy piece</returns>
    private static Space RandomEnemyPiece(Board board)
    {
        IList<Space> occupiedSpaces = new List<Space>();
        for (int x = 0; x < board.Width; x++)
        for (int y = 0; y < board.Height; y++)
        {
            Space space = board.GetSpace(new Vector2Int(x, y));
            if (space.HasEnemy(true)) occupiedSpaces.Add(space);
        }
        if (occupiedSpaces.Count < 1) return null;
        else return occupiedSpaces[UnityEngine.Random.Range(0, occupiedSpaces.Count - 1)];
    }
}
