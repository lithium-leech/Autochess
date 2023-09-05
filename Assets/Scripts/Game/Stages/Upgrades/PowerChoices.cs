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

    public override string TitleText { get; } = "ChoosePower";

    /// <summary>The available powers for the player</summary>
    private AssetGroup.Power[] PlayerPowers { get; }

    /// <summary>The available powers for the enemy</summary>
    private AssetGroup.Power[] EnemyPowers { get; }

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

    public override void ApplyChoice(int choice)
    {
        // Get the powers corresponding to the selected option
        AssetGroup.Power playerChoice = PlayerPowers[Game.ChoiceButtons.SelectedIndex];
        AssetGroup.Power enemyChoice = EnemyPowers[Game.ChoiceButtons.SelectedIndex];

        // Log the selected pieces
        Debug.Log($"Enemy Upgrade: {enemyChoice}");
        Debug.Log($"Player Upgrade: {playerChoice}");

        // Apply the player's new power
        Power playerPower = Game.CreatePower(playerChoice, true);
        Sprites.Remove(playerPower.gameObject);
        playerPower.Activate();

        // Apply the enemy's new power
        Power enemyPower = Game.CreatePower(enemyChoice, false);
        Sprites.Remove(enemyPower.gameObject);
        enemyPower.Activate();
    }

    /// <summary>Create a power sprite</summary>
    /// <param name="choice">The choice to create the sprite for</param>
    /// <param name="player">True if this is the player's half of the choice</param>
    /// <param name="info">True if this is the player's half of the choice</param>
    /// <returns>A new Piece</returns>
    private Power CreatePower(int choice, bool player, bool info)
    {
        // Create the new power
        Power power;
        if (player) power = Game.CreatePower(PlayerPowers[choice], true);
        else power = Game.CreatePower(EnemyPowers[choice], false);
        
        // Move it to the desired location
        if (info)
        {
            power.WarpTo(InfoPosition(player, false, false));
            InfoSprites.Add(power.gameObject);
        }
        else
        {
            power.WarpTo(ChoicePosition(choice, player, false));
            Sprites.Add(power.gameObject);
        }

        // Return the new power
        return power;
    }

    /// <summary>Returns a random choice of power</summary>
    /// <returns>A power choice</returns>
    private AssetGroup.Power GetRandomPower(bool player)
    {
        // Get the powers currently possessed
        IList<Power> currentPowers = player ? Game.PlayerPowers : Game.EnemyPowers;

        // Get the available powers
        IList<AssetGroup.Power> availablePowers = GetAvailablePowers(player, currentPowers);

        // Randomly return one available power
        int randomIndex = Random.Range(0, availablePowers.Count);
        return availablePowers[randomIndex];
    }

    /// <summary>Gets all of the powers that can be offered</summary>
    /// <param name="player">True if these are powers for the player</param>
    /// <param name="myPowers">The powers currently possessed</param>
    /// <returns>A list of available powers</returns>
    private IList<AssetGroup.Power> GetAvailablePowers(bool player, IEnumerable<Power> myPowers)
    {
        // Create an empty collection of powers
        IList<AssetGroup.Power> powers = new List<AssetGroup.Power>();
        
        // Only offer First to the black player
        if (player ^ GameState.IsPlayerWhite) powers.Add(AssetGroup.Power.First);

        // Only offer Row if there are available rows
        if (Game.GameBoard.PlayerRows + Game.GameBoard.EnemyRows < Game.GameBoard.Height - 2) powers.Add(AssetGroup.Power.Row);
        
        // Only offer up to 8 obstacles
        int obstacleCount = 0;
        foreach (Power power in myPowers)
        {
            switch(power.Kind)
            {
                case AssetGroup.Power.Mine:
                    obstacleCount++;
                    break;
                case AssetGroup.Power.Wall:
                    obstacleCount += 2;
                    break;
                default:
                    break;
            }
        }
        if (obstacleCount < 8)
        {
            powers.Add(AssetGroup.Power.Mine);
            powers.Add(AssetGroup.Power.Wall);
        }

        // Only offer up to 8 equipment
        int equipmentCount = 0;
        foreach (Power power in myPowers)
        {
            switch(power.Kind)
            {
                case AssetGroup.Power.Shield:
                    equipmentCount++;
                    break;
                default:
                    break;
            }
        }
        if (equipmentCount < 8)
        {
            powers.Add(AssetGroup.Power.Shield);
        }

        // Return the final collection
        return powers;
    }
}
