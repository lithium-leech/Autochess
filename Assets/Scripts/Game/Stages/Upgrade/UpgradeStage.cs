using System.Collections.Generic;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

/// <summary>
/// The stage of the game where the player chooses upgrades for the next level
/// </summary>
public class UpgradeStage : IStage
{
    /// <summary>Creates a new instance of an UpgradeStage</summary>
    /// <param name="game">The Game to run the UpgradeStage in</param>
    public UpgradeStage(Game game) => Game = game;

    /// <summary>The Game this is being run in</summary>
    private Game Game { get; set; }

    /// <summary>The next level choices the player is being offered</summary>
    public UpgradeChoices Choices { get; set; }

    public void Start()
    {
        // Start the planning music
        GameState.MusicBox.PlayMusic(SongName.Planning);

        // Go to the next level
        GameState.Level++;
        if (GameState.Level > GameState.HighScore) GameState.HighScore = GameState.Level;
        Game.LevelText.GetComponent<TextMeshProUGUI>().text = GameState.Level.ToString();
        PersistentVariablesSource pvs = LocalizationSettings.StringDatabase.SmartFormatter.GetSourceExtension<PersistentVariablesSource>();
        IntVariable score = pvs["game"]["score"] as IntVariable;
        IntVariable highscore = pvs["game"]["highscore"] as IntVariable;
        using (PersistentVariablesSource.UpdateScope())
        {
            score.Value = GameState.Level;
            highscore.Value = GameState.HighScore;
        }

        // Display new level choices
        if (GameState.Level % 20 == 1)
        {
            // Reset pieces
            Game.EnemyPieces.Clear();
            Game.PlayerGameBoard.Clear();
            Game.PlayerSideBoard.Clear();
            Game.EnemyPieces.Add(AssetGroup.Piece.Private);
            Game.PlayerSideBoard.Add(new PiecePositionRecord(AssetGroup.Piece.Private, null));
            
            // Reset powers
            IList<Power> powers = new List<Power>();
            foreach (Power power in Game.EnemyPowers) powers.Add(power);
            foreach (Power power in Game.PlayerPowers) powers.Add(power);
            foreach (Power power in powers) power.Deactivate();

            // Choose the next map
            Choices = new NullChoices(Game);
            Game.NextStage = new PlanningStage(Game);
            // TODO: MAPS
        }
        else if (GameState.Level % 5 == 0)
        {
            // Award a new power
            Choices = new PowerChoices(Game);
        }
        else
        {
            // Award a new piece
            Choices = new PieceChoices(Game);
        }
        Game.UpgradeMenuTitleText.StringReference = new LocalizedString("Text", Choices.TitleText);
        Choices.Show();
        Game.ChoiceButtons.SelectButton(-1);
        MenuManager.AddActiveMenu(Game.UpgradeMenu);

        // Add button listeners
        Game.ConfirmChoiceButton.onClick.AddListener(Confirm);
        for (int i = 0; i < Game.ChoiceButtons.Buttons.Length; i++)
        {
            int choice = i; // Capture the index in order to pass by value
            RadioButton button = Game.ChoiceButtons.Buttons[choice];
            button.onSelect.AddListener(() => Choices.ShowInfo(choice));
        }
    }

    public void During() { }

    public void End()
    {
        // Remove button listeners
        Game.ConfirmChoiceButton.onClick.RemoveAllListeners();
        for (int i = 0; i < Game.ChoiceButtons.Buttons.Length; i++)
        {
            int choice = i; // Capture the index in order to pass by value
            RadioButton button = Game.ChoiceButtons.Buttons[choice];
            button.onSelect.RemoveAllListeners();
        }

        // Default to the first choice if one was not selected
        if (Game.ChoiceButtons.SelectedIndex < 0) Game.ChoiceButtons.SelectedIndex = 0;

        // Apply the selected choice
        Choices.ApplyChoice(Game.ChoiceButtons.SelectedIndex);

        // Remove the upgrade menu
        Choices.Remove();
        MenuManager.RemoveActiveMenu(Game.UpgradeMenu);
    }

    /// <summary>Chooses one of the options</summary>
    /// <param name="choice">The option chosen</param>
    public void Confirm()
    {
        if (Game.ChoiceButtons.SelectedIndex > -1)
        {
            Game.NextStage = new PlanningStage(Game);
        }
    }
}
