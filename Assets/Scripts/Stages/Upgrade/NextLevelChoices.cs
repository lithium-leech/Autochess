using System.Collections.Generic;
using UnityEditor;
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
        PlayerPiece0 = GetRandomPiece();
        EnemyPiece0 = GetRandomPiece();
        PlayerPiece1 = GetRandomPiece();
        EnemyPiece1 = GetRandomPiece();
        PlayerPiece2 = GetRandomPiece();
        EnemyPiece2 = GetRandomPiece();
    }

    /// <summary>The piece the player gets if the first option is chosen</summary>
    public AssetGroup.Piece PlayerPiece0 { get; set; }

    /// <summary>The piece the enemy gets if the first option is chosen</summary>
    public AssetGroup.Piece EnemyPiece0 { get; set; }

    /// <summary>The piece the player gets if the second option is chosen</summary>
    public AssetGroup.Piece PlayerPiece1 { get; set; }

    /// <summary>The piece the enemy gets if the second option is chosen</summary>
    public AssetGroup.Piece EnemyPiece1 { get; set; }

    /// <summary>The piece the player gets if the third is chosen</summary>
    public AssetGroup.Piece PlayerPiece2 { get; set; }

    /// <summary>The piece the enemy gets if the third option is chosen</summary>
    public AssetGroup.Piece EnemyPiece2 { get; set; }

    /// <summary>Instantiated pieces shown in the upgrade menu</summary>
    private IList<Piece> Pieces { get; set; } = new List<Piece>();

    /// <summary>Instantiated panels shown in the upgrade menu</summary>
    private IList<GameObject> Panels { get; set; } = new List<GameObject>();
    
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
        GameObject playerTile0 = game.CreatePanel(false, false);
        playerTile0.transform.position = new Vector3(-2.125f, 2.25f, -22.0f);
        Panels.Add(playerTile0);
        Piece playerChoice0 = game.CreatePiece(PlayerPiece0, false);
        playerChoice0.WarpTo(new Vector3(-2.125f, 2.25f, -23.0f));
        playerChoice0.IsPlayerPiece = false;
        Pieces.Add(playerChoice0);

        GameObject enemyTile0 = game.CreatePanel(true, false);
        enemyTile0.transform.position = new Vector3(-2.125f, 3.25f, -22.0f);
        Panels.Add(enemyTile0);
        Piece enemyChoice0 = game.CreatePiece(EnemyPiece0, true);
        enemyChoice0.WarpTo(new Vector3(-2.125f, 3.25f, -23.0f));
        enemyChoice0.IsPlayerPiece = false;
        Pieces.Add(enemyChoice0);

        GameObject playerTile1 = game.CreatePanel(false, false);
        playerTile1.transform.position = new Vector3(0, 2.25f, -22.0f);
        Panels.Add(playerTile1);
        Piece playerChoice1 = game.CreatePiece(PlayerPiece1, false);
        playerChoice1.WarpTo(new Vector3(0, 2.25f, -23.0f));
        playerChoice1.IsPlayerPiece = false;
        Pieces.Add(playerChoice1);

        GameObject enemyTile1 = game.CreatePanel(true, false);
        enemyTile1.transform.position = new Vector3(0, 3.25f, -22.0f);
        Panels.Add(enemyTile1);
        Piece enemyChoice1 = game.CreatePiece(EnemyPiece1, true);
        enemyChoice1.WarpTo(new Vector3(0, 3.25f, -23.0f));
        enemyChoice1.IsPlayerPiece = false;
        Pieces.Add(enemyChoice1);

        GameObject playerTile2 = game.CreatePanel(false, false);
        playerTile2.transform.position = new Vector3(2.125f, 2.25f, -22.0f);
        Panels.Add(playerTile2);
        Piece playerChoice2 = game.CreatePiece(PlayerPiece2, false);
        playerChoice2.WarpTo(new Vector3(2.125f, 2.25f, -23.0f));
        playerChoice2.IsPlayerPiece = false;
        Pieces.Add(playerChoice2);

        GameObject enemyTile2 = game.CreatePanel(true, false);
        enemyTile2.transform.position = new Vector3(2.125f, 3.25f, -22.0f);
        Panels.Add(enemyTile2);
        Piece enemyChoice2 = game.CreatePiece(EnemyPiece2, true);
        enemyChoice2.WarpTo(new Vector3(2.125f, 3.25f, -23.0f));
        enemyChoice2.IsPlayerPiece = false;
        Pieces.Add(enemyChoice2);
    }

    /// <summary>Removes the pieces being shown in the upgrade menu</summary>
    public void RemovePiecesInMenu()
    {
        foreach(Piece piece in Pieces) piece.IsCaptured = true;
        Pieces.Clear();
    }

    /// <summary>Removes the panels being shown in the upgrade menu</summary>
    public void RemovePanelsInMenu()
    {
        foreach (GameObject panel in Panels) GameObject.Destroy(panel);
        Panels.Clear();
    }
}
