using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds information about the choices a player has when
/// they're going to the next level
/// </summary>
public class NextLevelChoices
{
    /// <summary>Creates a new set of next level choices</summary>
    public NextLevelChoices()
    {
        // Pick random pieces
        PlayerPiece1 = GetRandomPiece();
        EnemyPiece1 = GetRandomPiece();
        PlayerPiece2 = GetRandomPiece();
        EnemyPiece2 = GetRandomPiece();
        PlayerPiece3 = GetRandomPiece();
        EnemyPiece3 = GetRandomPiece();
    }

    /// <summary>The piece the player gets if option 1 is chosen</summary>
    public AssetGroup.Piece PlayerPiece1 { get; set; }

    /// <summary>The piece the enemy gets if option 1 is chosen</summary>
    public AssetGroup.Piece EnemyPiece1 { get; set; }

    /// <summary>The piece the player gets if option 2 is chosen</summary>
    public AssetGroup.Piece PlayerPiece2 { get; set; }

    /// <summary>The piece the enemy gets if option 2 is chosen</summary>
    public AssetGroup.Piece EnemyPiece2 { get; set; }

    /// <summary>The piece the player gets if option 3 is chosen</summary>
    public AssetGroup.Piece PlayerPiece3 { get; set; }

    /// <summary>The piece the enemy gets if option 3 is chosen</summary>
    public AssetGroup.Piece EnemyPiece3 { get; set; }

    /// <summary>Instantiated pieces shown in the upgrade menu</summary>
    private IList<Piece> Pieces { get; set; } = new List<Piece>();

    /// <summary>Returns a random type of piece</summary>
    /// <returns>A Piece type</returns>
    private AssetGroup.Piece GetRandomPiece()
    {
        return Random.Range(1, 7) switch
        {
            1 => AssetGroup.Piece.Pawn,
            2 => AssetGroup.Piece.Rook,
            3 => AssetGroup.Piece.Knight,
            4 => AssetGroup.Piece.Bishop,
            5 => AssetGroup.Piece.Queen,
            _ => AssetGroup.Piece.King
        };
    }

    /// <summary>Instantiates the choices as pieces so that they can be seen in the upgrade menu</summary>
    public void ShowPiecesInMenu(Game game)
    {
        Piece playerChoice1 = game.CreatePiece(PlayerPiece1, false);
        playerChoice1.IsPlayerPiece = false;
        playerChoice1.WarpTo(new Vector3(-2.25f, 1, -21));
        Pieces.Add(playerChoice1);
        
        Piece enemyChoice1 = game.CreatePiece(EnemyPiece1, true);
        enemyChoice1.IsPlayerPiece = false;
        enemyChoice1.WarpTo(new Vector3(-2.25f, 2, -21));
        Pieces.Add(enemyChoice1);

        Piece playerChoice2 = game.CreatePiece(PlayerPiece2, false);
        playerChoice2.IsPlayerPiece = false;
        playerChoice2.WarpTo(new Vector3(0, 1, -21));
        Pieces.Add(playerChoice2);

        Piece enemyChoice2 = game.CreatePiece(EnemyPiece2, true);
        enemyChoice2.IsPlayerPiece = false;
        enemyChoice2.WarpTo(new Vector3(0, 2, -21));
        Pieces.Add(enemyChoice2);

        Piece playerChoice3 = game.CreatePiece(PlayerPiece3, false);
        playerChoice3.IsPlayerPiece = false;
        playerChoice3.WarpTo(new Vector3(2.25f, 1, -21));
        Pieces.Add(playerChoice3);

        Piece enemyChoice3 = game.CreatePiece(EnemyPiece3, true);
        enemyChoice3.IsPlayerPiece = false;
        enemyChoice3.WarpTo(new Vector3(2.25f, 2, -21));
        Pieces.Add(enemyChoice3);
    }

    /// <summary>Removes the pieces being shown in the upgrade menu</summary>
    public void RemovePiecesInMenu()
    {
        foreach(Piece piece in Pieces) piece.IsCaptured = true;
    }
}
