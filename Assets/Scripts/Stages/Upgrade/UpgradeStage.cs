using System;
using TMPro;
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
    public NextLevelChoices Choices { get; set; }

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
        Choices = new NextLevelChoices();
        Choices.ShowPiecesInMenu(Game);
        Game.ChoiceButtons.SelectButton(-1);
        MenuManager.AddActiveMenu(Game.UpgradeMenu);

        // Delete any pieces in the trash
        foreach (Piece piece in Game.TrashBoard.PlayerPieces)
            piece.IsCaptured= true;

        // Add button listeners
        Game.ConfirmChoiceButton.onClick.AddListener(Confirm);
    }

    public void During() { }

    public void End()
    {
        // Remove button listeners
        Game.ConfirmChoiceButton.onClick.RemoveAllListeners();

        // Default to the first choice if one was not selected
        if (Game.ChoiceButtons.SelectedIndex < 0) Game.ChoiceButtons.SelectedIndex = 0;

        // Get the pieces corresponding to the selected option
        AssetGroup.Piece playerPiece;
        AssetGroup.Piece enemyPiece;
        if (Game.ChoiceButtons.SelectedIndex == 0)
        {
            playerPiece = Choices.PlayerPiece0;
            enemyPiece = Choices.EnemyPiece0;
        }
        else if (Game.ChoiceButtons.SelectedIndex == 1)
        {
            playerPiece = Choices.PlayerPiece1;
            enemyPiece = Choices.EnemyPiece1;
        }
        else if (Game.ChoiceButtons.SelectedIndex == 2)
        {
            playerPiece = Choices.PlayerPiece2;
            enemyPiece = Choices.EnemyPiece2;
        }
        else
        {
            throw new Exception($"Next level choice {Game.ChoiceButtons.SelectedIndex} not recognized");
        }

        // Add the enemy piece, removing a lower value piece if necessary
        if (Game.EnemyPieces.Count == 16) RemoveLowerValue(enemyPiece);
        if (Game.EnemyPieces.Count < 16) Game.EnemyPieces.Add(enemyPiece);

        // Add the player piece as long a sideboard space is empty
        if (Game.PlayerSideBoard.Count < 16) Game.PlayerSideBoard.Add(new PositionRecord(playerPiece, null));

        // Remove the upgrade menu
        Choices.RemovePiecesInMenu();
        Choices.RemovePanelsInMenu();
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
