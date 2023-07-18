using System.Collections.Generic;
using System.Linq;
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
        PlayerPowers = new PowerChoice[NumberOfChoices];
        EnemyPowers = new PowerChoice[NumberOfChoices];
        for (int i = 0; i < NumberOfChoices; i++)
        {
            PlayerPowers[i] = GetRandomPower(true);
            EnemyPowers[i] = GetRandomPower(false);
        }
    }

    public override string TitleText { get; } = "ChoosePower";

    /// <summary>The available powers for the player</summary>
    private PowerChoice[] PlayerPowers { get; }

    /// <summary>The available powers for the enemy</summary>
    private PowerChoice[] EnemyPowers { get; }

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
        PowerChoice playerChoice = PlayerPowers[Game.ChoiceButtons.SelectedIndex];
        PowerChoice enemyChoice = EnemyPowers[Game.ChoiceButtons.SelectedIndex];

        // Apply the player's new power
        if (playerChoice.IsRemove)
        {
            Power power = Game.EnemyPowers.First(p => p.Kind == playerChoice.Kind);
            power.Deactivate();
        }
        else
        {
            Power power = Game.CreatePower(playerChoice.Kind, true, false);
            Powers.Remove(power);
            power.Activate();
        }

        // Apply the enemy's new power
        if (enemyChoice.IsRemove)
        {
            Power power = Game.PlayerPowers.First(p => p.Kind == enemyChoice.Kind);
            power.Deactivate();
        }
        else
        {
            Power power = Game.CreatePower(enemyChoice.Kind, false, false);
            Powers.Remove(power);
            power.Activate();
        }
    }

    /// <summary>Create a power sprite</summary>
    /// <param name="choice">The choice to create the sprite for</param>
    /// <param name="player">True if this is the player's half of the choice</param>
    /// <param name="info">True if this is the player's half of the choice</param>
    /// <returns>A new Piece</returns>
    private Power CreatePower(int choice, bool player, bool info)
    {
        Power power;
        if (player)
        {
            power = Game.CreatePower(PlayerPowers[choice].Kind, true, PlayerPowers[choice].IsRemove);
        }
        else
        {
            power = Game.CreatePower(EnemyPowers[choice].Kind, false, EnemyPowers[choice].IsRemove);
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

    /// <summary>Returns a random choice of power</summary>
    /// <returns>A power choice</returns>
    private PowerChoice GetRandomPower(bool player)
    {
        // Determine who is gaining a power
        IList<Power> myPowers = player ? Game.PlayerPowers : Game.EnemyPowers;
        IList<Power> theirPowers = player ? Game.EnemyPowers : Game.PlayerPowers;

        // Roll to remove a power
        if (theirPowers.Count > 0 && Random.Range(1,6) == 5)
        {
            int removeIndex = Random.Range(0, theirPowers.Count);
            return new PowerChoice(theirPowers[removeIndex].Kind, true);
        }

        // Determine the available powers
        IList<AssetGroup.Power> availablePowers = new List<AssetGroup.Power>();
        
        // Only offer First to the black player
        if (player ^ GameState.IsPlayerWhite) availablePowers.Add(AssetGroup.Power.First);

        // Only offer Row if there are available rows
        if (Game.GameBoard.PlayerRows + Game.GameBoard.EnemyRows < Game.GameBoard.Height) availablePowers.Add(AssetGroup.Power.Row);
        
        // Offer the remaining powers
        availablePowers.Add(AssetGroup.Power.Mine);
        availablePowers.Add(AssetGroup.Power.Wall);
        availablePowers.Add(AssetGroup.Power.Shield);

        // Randomly return one of the available powers
        int randomIndex = Random.Range(0, availablePowers.Count);
        return new PowerChoice(availablePowers[randomIndex], false);
    }

    /// <summary>A single selectable choice</summary>
    private class PowerChoice
    {
        /// <summary>Creates a new instance of a PowerChoice</summary>
        /// <param name="kind">The kind of power</param>
        /// <param name="remove">True if this is a power removal</param>
        public PowerChoice(AssetGroup.Power kind, bool remove)
        {
            Kind = kind;
            IsRemove = remove;
        }

        /// <summary>The kind of power</summary>
        public AssetGroup.Power Kind { get; }

        /// <summary>True if this is a power removal</summary>
        public bool IsRemove { get; }
    }
}
