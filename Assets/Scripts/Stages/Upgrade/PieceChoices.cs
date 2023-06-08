using System;
using System.Collections.Generic;
using UnityEngine.Localization;

/// <summary>
/// Holds information about the choices a player has when
/// they're choosing pieces for the next level
/// </summary>
public class PieceChoices : UpgradeChoices
{
    /// <summary>Creates a new set of piece choices</summary>
    public PieceChoices(Game game) : base(game)
    {
        // Pick random pieces
        PlayerPieces = new AssetGroup.Piece[NumberOfChoices];
        EnemyPieces = new AssetGroup.Piece[NumberOfChoices];
        for (int i = 0; i < NumberOfChoices; i++)
        {
            PlayerPieces[i] = GetRandomPiece();
            EnemyPieces[i] = GetRandomPiece();
        }
    }

    /// <summary>The available powers for the player</summary>
    public AssetGroup.Piece[] PlayerPieces { get; }

    /// <summary>The available powers for the enemy</summary>
    public AssetGroup.Piece[] EnemyPieces { get; }

    /// <summary>Instantiated pieces shown in the upgrade menu</summary>
    private IList<Piece> Pieces { get; set; } = new List<Piece>();
    
    /// <summary>Instantiated pieces shown in the info section</summary>
    private IList<Piece> InfoPieces { get; set; } = new List<Piece>();

    protected override void ShowChoices()
    {
        for (int i = 0; i < NumberOfChoices; i++)
        {
            CreatePiece(i, true, false);
            CreatePiece(i, false, false);
        }
    }

    public override void ShowInfo(int choice)
    {
        // Erase previous info
        RemoveInfo();

        // Create new sprites
        Piece playerPiece = CreatePiece(choice, true, true);
        Piece enemyPiece = CreatePiece(choice, false, true);

        // Change text
        Game.PlayerChoiceNameText.StringReference = new LocalizedString("PieceNames", $"{playerPiece.Kind}");
        Game.PlayerChoiceInfoText.StringReference = new LocalizedString("PieceInfo", $"{playerPiece.Kind}");
        Game.EnemyChoiceNameText.StringReference = new LocalizedString("PieceNames", $"{enemyPiece.Kind}");
        Game.EnemyChoiceInfoText.StringReference = new LocalizedString("PieceInfo", $"{enemyPiece.Kind}");
    }

    protected override void RemoveChoices()
    {
        foreach (Piece piece in Pieces) piece.IsCaptured = true;
        Pieces.Clear();
    }

    protected override void RemoveInfo()
    {
        // Remove sprites
        foreach (Piece piece in InfoPieces) piece.IsCaptured = true;
        InfoPieces.Clear();

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
        // Get the pieces corresponding to the selected option
        AssetGroup.Piece playerPiece;
        AssetGroup.Piece enemyPiece;
        playerPiece = PlayerPieces[Game.ChoiceButtons.SelectedIndex];
        enemyPiece = EnemyPieces[Game.ChoiceButtons.SelectedIndex];

        // Add the enemy piece, removing a lower value piece if necessary
        if (Game.EnemyPieces.Count == 16) RemoveLowerValue(enemyPiece);
        if (Game.EnemyPieces.Count < 16) Game.EnemyPieces.Add(enemyPiece);

        // Add the player piece as long a sideboard space is empty
        if (Game.PlayerSideBoard.Count < 16) Game.PlayerSideBoard.Add(new PositionRecord(playerPiece, null));
    }

    /// <summary>Create a piece sprite</summary>
    /// <param name="choice">The choice to create the sprite for</param>
    /// <param name="player">True if this is the player's half of the choice</param>
    /// <param name="info">True if this is going to be shown in the info menu</param>
    /// <returns>A new Piece</returns>
    private Piece CreatePiece(int choice, bool player, bool info)
    {
        Piece piece;
        if (player)
        {
            piece = Game.CreatePiece(PlayerPieces[choice], false);
        }
        else
        {
            piece = Game.CreatePiece(EnemyPieces[choice], true);
        }
        piece.IsPlayerPiece = player;
        if (info)
        {
            piece.WarpTo(InfoPosition(player, false, false));
            InfoPieces.Add(piece);
        }
        else
        {
            piece.WarpTo(ChoicePosition(choice, player, false));
            Pieces.Add(piece);
        }
        return piece;
    }

    /// <summary>Returns a random type of piece</summary>
    /// <returns>A Piece type</returns>
    private AssetGroup.Piece GetRandomPiece()
    {
        return UnityEngine.Random.Range(1, 7) switch
        {
            1 => AssetGroup.Piece.Pawn,
            2 => AssetGroup.Piece.Rook,
            3 => AssetGroup.Piece.Knight,
            4 => AssetGroup.Piece.Bishop,
            5 => AssetGroup.Piece.Queen,
            6 => AssetGroup.Piece.King,
            _ => throw new Exception($"A random selection in PieceChoices was not implemented")
        }; ;
    }

    /// <summary>Remove an enemy piece with lower value than the given kind</summary>
    /// <param name="kind">The kind of piece to make space for</param>
    public void RemoveLowerValue(AssetGroup.Piece kind)
    {
        int valueToAdd = GetPieceValue(kind);
        for (int i = 0; i < Game.EnemyPieces.Count; i++)
        {
            int valueToRemove = GetPieceValue(Game.EnemyPieces[i]);
            if (valueToRemove < valueToAdd)
            {
                Game.EnemyPieces.RemoveAt(i);
                return;
            }
        }
    }

    /// <summary>Get the value of a given kind of piece</summary>
    /// <param name="kind">The kind of piece to evaluate</param>
    /// <returns>An integer value</returns>
    public int GetPieceValue(AssetGroup.Piece kind)
    {
        return kind switch
        {
            AssetGroup.Piece.Pawn => 0,
            AssetGroup.Piece.Knight => 1,
            AssetGroup.Piece.King => 2,
            AssetGroup.Piece.Bishop => 3,
            AssetGroup.Piece.Rook => 4,
            AssetGroup.Piece.Queen => 5,
            _ => throw new Exception($"Piece kind {kind} not recognized")
        };
    }
}
