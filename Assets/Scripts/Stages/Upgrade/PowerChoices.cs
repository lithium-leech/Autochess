using System.Collections.Generic;
using UnityEngine;
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
        // Pick random powers
        PlayerPowers = new AssetGroup.Power[NumberOfChoices];
        EnemyPowers = new AssetGroup.Power[NumberOfChoices];
        for (int i = 0; i < NumberOfChoices; i++)
        {
            PlayerPowers[i] = GetRandomPower(true);
            EnemyPowers[i] = GetRandomPower(false);
        }
    }

    /// <summary>The available powers for the player</summary>
    public AssetGroup.Power[] PlayerPowers { get; }

    /// <summary>The available powers for the enemy</summary>
    public AssetGroup.Power[] EnemyPowers { get; }

    /// <summary>Instantiated powers shown in the upgrade menu</summary>
    private IList<Power> Powers { get; set; } = new List<Power>();

    /// <summary>Instantiated powers shown in the info section</summary>
    private IList<Power> InfoPowers { get; set; } = new List<Power>();

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
        Power playerPower = CreatePower(choice, true, true);
        Power enemyPower = CreatePower(choice, false, true);

        // Change text
        Game.PlayerChoiceNameText.StringReference = new LocalizedString("PowerNames", $"{playerPower.Kind}");
        Game.PlayerChoiceInfoText.StringReference = new LocalizedString("PowerInfo", $"{playerPower.Kind}");
        Game.EnemyChoiceNameText.StringReference = new LocalizedString("PowerNames", $"{enemyPower.Kind}");
        Game.EnemyChoiceInfoText.StringReference = new LocalizedString("PowerInfo", $"{enemyPower.Kind}");
    }

    protected override void RemoveChoices()
    {
        foreach (Power power in Powers) GameObject.Destroy(power.gameObject);
        Powers.Clear();
    }

    protected override void RemoveInfo()
    {
        // Remove sprites
        foreach (Power power in InfoPowers) GameObject.Destroy(power.gameObject);
        InfoPowers.Clear();

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
        // Get the powers corresponding to the selected option
        AssetGroup.Power playerKind;
        AssetGroup.Power enemyKind;
        playerKind = PlayerPowers[Game.ChoiceButtons.SelectedIndex];
        enemyKind = EnemyPowers[Game.ChoiceButtons.SelectedIndex];

        // Create temporary power objects
        Power playerPower = Game.CreatePower(playerKind, true);
        Power enemyPower = Game.CreatePower(enemyKind, false);

        // Apply the powers
        playerPower.Activate();
        enemyPower.Activate();

        // Delete the powers
        Game.Destroy(playerPower.gameObject);
        Game.Destroy(enemyPower.gameObject);
    }

    /// <summary>Create a power sprite</summary>
    /// <param name="choice">The choice to create the sprite for</param>
    /// <param name="player">True if this is the player's half of the choice</param>
    /// <returns>A new Piece</returns>
    private Power CreatePower(int choice, bool player, bool info)
    {
        Power power;
        if (player)
        {
            power = Game.CreatePower(PlayerPowers[choice], true);
        }
        else
        {
            power = Game.CreatePower(EnemyPowers[choice], false);
        }
        if (info)
        {
            power.WarpTo(InfoPosition(player, false, false));
            InfoPowers.Add(power);
        }
        else
        {
            power.WarpTo(ChoicePosition(choice, player, false));
            Powers.Add(power);
        }
        return power;
    }

    /// <summary>Returns a random type of power</summary>
    /// <returns>A Power type</returns>
    private AssetGroup.Power GetRandomPower(bool player)
    {
        // Determine the available powers
        IList<AssetGroup.Power> availablePowers = new List<AssetGroup.Power>();
        
        // Only offer this power to the black player
        if (player && !GameState.IsPlayerWhite) availablePowers.Add(AssetGroup.Power.First);
        else if (!player && GameState.IsPlayerWhite) availablePowers.Add(AssetGroup.Power.First);

        availablePowers.Add(AssetGroup.Power.ExtraRow);
        availablePowers.Add(AssetGroup.Power.Mine);
        availablePowers.Add(AssetGroup.Power.Walls);
        availablePowers.Add(AssetGroup.Power.Shield);

        // Randomly return one of the available powers
        int randomIndex = Random.Range(0, availablePowers.Count);
        return availablePowers[randomIndex];
    }
}
