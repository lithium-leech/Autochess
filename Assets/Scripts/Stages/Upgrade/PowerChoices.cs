using System;
using System.Collections.Generic;
using UnityEngine.Localization;

/// <summary>
/// Holds information about the choices a player has when
/// they're choosing powers for the next level
/// </summary>
public class PowerChoices : UpgradeChoices
{
    /// <summary>Creates a new set of power choices</summary>
    public PowerChoices(Game game) : base(game)
    {
        // Pick random pieces
        //PlayerPieces = new AssetGroup.Piece[NumberOfChoices];
        //EnemyPieces = new AssetGroup.Piece[NumberOfChoices];
        for (int i = 0; i < NumberOfChoices; i++)
        {
            //PlayerPieces[i] = GetRandomPiece();
            //EnemyPieces[i] = GetRandomPiece();
        }
    }

    /// <summary>The available powers for the player</summary>
    public AssetGroup.Piece[] PlayerPowers { get; }

    /// <summary>The available powers for the enemy</summary>
    public AssetGroup.Piece[] EnemyPowers { get; }

    /// <summary>Instantiated powers shown in the upgrade menu</summary>
    private IList<Piece> Powers { get; set; } = new List<Piece>();

    /// <summary>Instantiated powers shown in the info section</summary>
    private IList<Piece> InfoPowers { get; set; } = new List<Piece>();

    protected override void ShowChoices()
    {
        for (int i = 0; i < NumberOfChoices; i++)
        {
            CreatePower(i, true, false);
            CreatePower(i, false, false);
        }
    }

    public override void ShowInfo(int choice)
    {
        // Erase previous info
        RemoveInfo();

        // Create new sprites
        Piece playerPiece = CreatePower(choice, true, true);
        Piece enemyPiece = CreatePower(choice, false, true);

        // Change text
        Game.PlayerChoiceNameText.StringReference = new LocalizedString("PieceNames", $"{playerPiece.Kind}");
        Game.PlayerChoiceInfoText.StringReference = new LocalizedString("PieceInfo", $"{playerPiece.Kind}");
        Game.EnemyChoiceNameText.StringReference = new LocalizedString("PieceNames", $"{enemyPiece.Kind}");
        Game.EnemyChoiceInfoText.StringReference = new LocalizedString("PieceInfo", $"{enemyPiece.Kind}");
    }

    protected override void RemoveChoices()
    {
        //foreach (Piece piece in Powers) piece.IsCaptured = true;
        //Pieces.Clear();
    }

    protected override void RemoveInfo()
    {
        // Remove sprites
        //foreach (Piece piece in InfoPieces) piece.IsCaptured = true;
        //InfoPieces.Clear();

        // Remove text
        Game.PlayerChoiceNameText.StringReference = new LocalizedString();
        Game.PlayerChoiceInfoText.StringReference = new LocalizedString();
        Game.EnemyChoiceNameText.StringReference = new LocalizedString();
        Game.EnemyChoiceInfoText.StringReference = new LocalizedString();
        Game.PlayerChoiceNameText.Text = string.Empty;
        Game.PlayerChoiceInfoText.Text = string.Empty;
        Game.EnemyChoiceNameText.Text = string.Empty;
        Game.EnemyChoiceInfoText.Text = string.Empty;
    }

    public override void ApplyChoice(int choice)
    {
        throw new Exception("Powers have not yet been implemented");
    }

    /// <summary>Create a power sprite</summary>
    /// <param name="choice">The choice to create the sprite for</param>
    /// <param name="player">True if this is the player's half of the choice</param>
    /// <returns>A new Piece</returns>
    private Piece CreatePower(int choice, bool player, bool info)
    {
        throw new Exception("Powers have not yet been implemented");
    }

    /// <summary>Returns a random type of piece</summary>
    /// <returns>A Piece type</returns>
    private AssetGroup.Piece GetRandomPower()
    {
        return UnityEngine.Random.Range(1, 7) switch
        {
            _ => throw new Exception($"A random selection in PowerChoices was not implemented")
        }; ;
    }
}
