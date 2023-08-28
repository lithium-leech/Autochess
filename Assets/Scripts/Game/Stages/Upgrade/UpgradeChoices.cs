using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds information about the choices a player has when
/// they're choosing pieces for the next level
/// </summary>
public abstract class UpgradeChoices
{
    /// <summary>Creates a new set of upgrade choices</summary>
    /// <remarks>Should be called at the start of an inherited constructor</remarks>
    protected UpgradeChoices(Game game)
    {
        Game = game;
        NumberOfChoices = game.ChoiceButtons.Buttons.Length;
    }

    /// <summary>The localization key for the title text to show along with this choice</summary>
    public abstract string TitleText { get; }

    /// <summary>The game to show the choices in</summary>
    protected Game Game { get; }

    /// <summary>The number of available choices</summary>
    protected int NumberOfChoices { get; }

    /// <summary>Instantiated panels shown in the upgrade menu</summary>
    private IList<GameObject> Panels { get; set; } = new List<GameObject>();

    public void Show()
    {
        ShowPanels();
        ShowChoices();
    }

    public void Remove()
    {
        RemoveInfo();
        RemoveChoices();
        RemovePanels();
    }

    /// <summary>Creates all of the background panels in the upgrade menu</summary>
    private void ShowPanels()
    {
        // Create choice backgrounds
        AssetGroup.Tile topTile = GameState.IsPlayerWhite ? AssetGroup.Tile.TrueBlackSpace : AssetGroup.Tile.TrueWhiteSpace;
        AssetGroup.Tile bottomTile = GameState.IsPlayerWhite ? AssetGroup.Tile.TrueWhiteSpace : AssetGroup.Tile.TrueBlackSpace;
        for (int i = 0; i < NumberOfChoices; i++)
        {
            Panels.Add(Game.CreateTile(topTile, ChoicePosition(i, false, true)));
            Panels.Add(Game.CreateTile(bottomTile, ChoicePosition(i, true, true)));
        }

        // Create info backgrounds
        AssetGroup.Tile rightTile = GameState.IsPlayerWhite ? AssetGroup.Tile.TrueBlackSpace : AssetGroup.Tile.TrueWhiteSpace;
        AssetGroup.Tile leftTile = GameState.IsPlayerWhite ? AssetGroup.Tile.TrueWhiteSpace : AssetGroup.Tile.TrueBlackSpace;
        AssetGroup.Panel rightPanel = GameState.IsPlayerWhite ? AssetGroup.Panel.BlackInfo : AssetGroup.Panel.WhiteInfo;
        AssetGroup.Panel leftPanel = GameState.IsPlayerWhite ? AssetGroup.Panel.WhiteInfo : AssetGroup.Panel.BlackInfo;
        Panels.Add(Game.CreateTile(rightTile, InfoPosition(false, true, false)));
        Panels.Add(Game.CreateTile(leftTile, InfoPosition(true, true, false)));
        Panels.Add(Game.CreatePanel(rightPanel, InfoPosition(false, true, true), Game.InfoMenu.transform));
        Panels.Add(Game.CreatePanel(leftPanel, InfoPosition(true, true, true), Game.InfoMenu.transform));
    }

    /// <summary>Creates sprites for the choices in the upgrade menu</summary>
    protected abstract void ShowChoices();

    /// <summary>Displays info for the given choice in the upgrade menu</summary>
    /// <param name="choice">The choice to show info for</param>
    public abstract void ShowInfo(int choice);

    /// <summary>Removes the panels being shown in the upgrade menu</summary>
    public void RemovePanels()
    {
        foreach (GameObject panel in Panels) GameObject.Destroy(panel);
        Panels.Clear();
    }

    /// <summary>Removes the sprites being shown in the upgrade menu</summary>
    protected abstract void RemoveChoices();

    /// <summary>Removes info being displayed in the upgrade menu</summary>
    protected abstract void RemoveInfo();

    /// <summary>Applies the given choice to the game</summary>
    /// <param name="choice">The choice to apply</param>
    public abstract void ApplyChoice(int choice);

    /// <summary>Gets the position of the desired choice display</summary>
    /// <param name="choice">The choice index</param>
    /// <param name="player">True if this is the player half of the choice</param>
    /// <param name="back">True if this is the backdrop</param>
    /// <returns>A Vector3</returns>
    protected Vector3 ChoicePosition(int choice, bool player, bool back)
    {
        float x = choice switch
        {
            0 => -2.125f,
            1 => 0.0f,
            2 => 2.125f,
            _ => throw new Exception("A choice index was not implemented in ChoicePosition")
        };
        float y = player ? 2.25f : 3.25f;
        float z = back ? -32.0f : -33.0f;
        return new Vector3(x, y, z);
    }

    /// <summary>Gets the position of the desired info display</summary>
    /// <param name="player">True if this is the player half of the info</param>
    /// <param name="back">True if this is the backdrop</param>
    /// <param name="text">True if this is the text panel</param>
    /// <returns>A Vector3</returns>
    protected Vector3 InfoPosition(bool player, bool back, bool text)
    {
        float dx = text ? 2.0f : 0.5f;
        float x = player ? -dx : dx;
        float y = text ? -5.75f : -4.0f;
        float z = text ? -31.0f : back ? -33.0f : -34.0f;
        return new Vector3(x, y, z);
    }
}
