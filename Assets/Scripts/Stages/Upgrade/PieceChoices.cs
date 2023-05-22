using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// Holds information about the choices a player has when
/// they're choosing pieces for the next level
/// </summary>
public class PieceChoices
{
    /// <summary>Creates a new set of piece choices</summary>
    public PieceChoices(Game game)
    {
        Game = game;
        NumberOfChoices = game.ChoiceButtons.Buttons.Length;

        // Pick random pieces
        PlayerPieces = new AssetGroup.Piece[NumberOfChoices];
        EnemyPieces = new AssetGroup.Piece[NumberOfChoices];
        for (int i = 0; i < NumberOfChoices; i++)
        {
            PlayerPieces[i] = GetRandomPiece();
            EnemyPieces[i] = GetRandomPiece();
        }
    }

    /// <summary>The piece the player gets if the first option is chosen</summary>
    public AssetGroup.Piece[] PlayerPieces { get; }

    /// <summary>The piece the enemy gets if the first option is chosen</summary>
    public AssetGroup.Piece[] EnemyPieces { get; }

    /// <summary>The game to show the choices in</summary>
    private Game Game { get; }

    /// <summary>The number of available choices</summary>
    private int NumberOfChoices { get; }

    /// <summary>Instantiated panels shown in the upgrade menu</summary>
    private IList<GameObject> Panels { get; set; } = new List<GameObject>();
    
    /// <summary>Instantiated pieces shown in the upgrade menu</summary>
    private IList<Piece> Pieces { get; set; } = new List<Piece>();
    
    /// <summary>Instantiated pieces shown in the info section</summary>
    private IList<Piece> InfoPieces { get; set; } = new List<Piece>();

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

    /// <summary>Creates all of the background panels in the upgrade menu</summary>
    public void ShowPanels()
    {
        // Create choice panels
        for (int i = 0; i < NumberOfChoices; i++)
        {
            CreatePanel(i, true, false, false);
            CreatePanel(i, false, false, false);
        }

        // Create info panels
        CreatePanel(-1, true, true, false);
        CreatePanel(-1, false, true, false);
        CreatePanel(-1, true, true, true);
        CreatePanel(-1, false, true, true);
    }

    /// <summary>Creates piece sprites for the choices in the upgrade menu</summary>
    public void ShowChoices()
    {
        for (int i = 0; i < NumberOfChoices; i++)
        {
            CreatePiece(i, true, false);
            CreatePiece(i, false, false);
        }
    }

    /// <summary>Displays info for the given choice in the upgrade menu</summary>
    /// <param name="choice">The choice to show info for</param>
    public void ShowInfo(int choice)
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

    /// <summary>Removes the pieces being shown in the upgrade menu</summary>
    public void RemovePiecesInMenu()
    {
        foreach (Piece piece in Pieces) piece.IsCaptured = true;
        Pieces.Clear();
    }

    /// <summary>Removes the panels being shown in the upgrade menu</summary>
    public void RemovePanelsInMenu()
    {
        foreach (GameObject panel in Panels) GameObject.Destroy(panel);
        Panels.Clear();
    }

    /// <summary>Removes info being displayed in the upgrade menu</summary>
    public void RemoveInfo()
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

    /// <summary>Create a panel sprite</summary>
    /// <param name="choice">The choice to create the sprite for</param>
    /// <param name="player">True if this is the player's half of the choice</param>
    /// <param name="player">True if this is an info panel</param>
    /// <param name="player">True if this the text panel</param>
    /// <returns>A new panel</returns>
    private GameObject CreatePanel(int choice, bool player, bool info, bool text)
    {
        GameObject panel = Game.CreatePanel(!player, text);
        if (text)
        {
            panel.transform.SetParent(Game.InfoMenu.transform);
            panel.transform.SetAsFirstSibling();
        }
        if (info)
        {
            panel.transform.position = InfoPosition(player, true, text);
        }
        else
        {
            panel.transform.position = ChoicePosition(choice, player, true);
        }
        Panels.Add(panel);
        return panel;
    }

    /// <summary>Create a piece sprite</summary>
    /// <param name="choice">The choice to create the sprite for</param>
    /// <param name="player">True if this is the player's half of the choice</param>
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

    /// <summary>Gets the position of the desired choice display</summary>
    /// <param name="choice">The choice index</param>
    /// <param name="player">True if this is the player half of the choice</param>
    /// <param name="panel">True if this is the background panel</param>
    /// <returns>A Vector3</returns>
    private Vector3 ChoicePosition(int choice, bool player, bool panel)
    {
        float x = choice switch
        {
            0 => -2.125f,
            1 => 0.0f,
            2 => 2.125f,
            _ => throw new Exception("A choice index was not implemented in PieceChoices")
        };
        float y = player ? 2.25f : 3.25f;
        float z = panel ? -32.0f : -33.0f;
        return new Vector3(x, y, z);
    }

    /// <summary>Gets the position of the desired info display</summary>
    /// <param name="player">True if this is the player half of the info</param>
    /// <param name="panel">True if this is the background panel</param>
    /// <param name="text">True if this is the text panel</param>
    /// <returns>A Vector3</returns>
    private Vector3 InfoPosition(bool player, bool panel, bool text)
    {
        float dx = text ? 2.0f : 0.5f;
        float x = player ? -dx : dx;
        float y = text ? -5.625f : -3.875f;
        float z = text ? -31.0f : panel ? -33.0f : -34.0f;
        return new Vector3(x, y, z);
    }
}
