using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

/// <summary>
/// The main game object, responsible for managing
/// the state of the game
/// </summary>
public class Game : MonoBehaviour
{
    /// Properties to set using Unity interface
    public TextMeshProUGUI LevelText;
    public TextLocalizer UpgradeMenuTitleText;
    public TextLocalizer PlayerChoiceNameText;
    public TextLocalizer PlayerChoiceInfoText;
    public TextLocalizer EnemyChoiceNameText;
    public TextLocalizer EnemyChoiceInfoText;
    public BasicButton FightButton;
    public BasicButton CancelConcedeButton;
    public BasicButton ConfirmConcedeButton;
    public RadioButtonGroup ChoiceButtons;
    public BasicButton ConfirmChoiceButton;
    public RewardedAdsButton RetryButton;
    public BasicButton EndGameButton;
    public GameObject InGameMenu;
    public GameObject ConcedeMenu;
    public GameObject UpgradeMenu;
    public GameObject InfoMenu;
    public GameObject GameOverMenu;
    public GameObject SettingsMenu;

    /// <summary>The current stage of the game being run</summary>
    public IStage CurrentStage { get; set; } = null;

    /// <summary>The next stage of the game to run</summary>
    /// <remarks>null when the current stage should keep running</remarks>
    public IStage NextStage { get; set; } = null;

    /// <summary>The enemy's roster of pieces (indexes to prefabs)</summary>
    public IList<AssetGroup.Piece> EnemyPieces { get; } = new List<AssetGroup.Piece>();

    /// <summary>The enemy's collection of miscellaneous objects</summary>
    public IList<AssetGroup.Object> EnemyObjects { get; } = new List<AssetGroup.Object>();

    /// <summary>The enemy's accumulated power ups</summary>
    public IList<Power> EnemyPowers { get; } = new List<Power>();

    /// <summary>The player's pieces on the game board</summary>
    public IList<PiecePositionRecord> PlayerGameBoard { get; } = new List<PiecePositionRecord>();

    /// <summary>The player's pieces on the side board</summary>
    public IList<PiecePositionRecord> PlayerSideBoard { get; } = new List<PiecePositionRecord>();

    /// <summary>The players's collection of miscellaneous objects</summary>
    public IList<AssetGroup.Object> PlayerObjects { get; } = new List<AssetGroup.Object>();

    /// <summary>The player's accumulated power ups</summary>
    public IList<Power> PlayerPowers { get; } = new List<Power>();

    /// <summary>The board that fights take place on</summary>
    public Board GameBoard { get; set; }

    /// <summary>The board that holds the player's available pieces</summary>
    public Board SideBoard { get; set; }

    /// <summary>The board for discarding unwanted pieces</summary>
    public Board TrashBoard { get; set; }

    private void Start()
    {
        // Initialize the menu manager
        MenuManager.Initialize(InGameMenu);

        // Go to the first level
        GameState.IsPlayerWhite = false;
        GameState.Level = 1;
        LevelText.text = GameState.Level.ToString();
        PersistentVariablesSource pvs = LocalizationSettings.StringDatabase.SmartFormatter.GetSourceExtension<PersistentVariablesSource>();
        IntVariable score = pvs["game"]["score"] as IntVariable;
        IntVariable highscore = pvs["game"]["highscore"] as IntVariable;
        using (PersistentVariablesSource.UpdateScope())
        {
            score.Value = GameState.Level;
            highscore.Value = GameState.HighScore;
        }

        // Create the game boards
        GameBoard = new Board(this, 8, 8, 2, 2, new Vector2(-4.0f, -1.0f));
        SideBoard = new Board(this, 8, 3, 3, 0, new Vector2(-4.0f, -5.75f));
        TrashBoard = new Board(this, 1, 1, 1, 0, new Vector2(-3.0f, -8.5f));

        // Create a sample setup;
        EnemyPieces.Add(AssetGroup.Piece.Pawn);
        PlayerSideBoard.Add(new PiecePositionRecord(AssetGroup.Piece.Pawn, null));

        // Start the planning phase
        NextStage = new PlanningStage(this);
    }

    private void Update()
    {
        // Check if a new stage has been queued
        if (NextStage != null)
        {
            // Replace the current stage with the next one
            Debug.Log($"Switching to {NextStage.GetType()}.");
            CurrentStage?.End();
            CurrentStage = NextStage;
            NextStage = null;
            CurrentStage.Start();
        }
        else
        {
            // Otherwise just run the current stage
            CurrentStage?.During();
        }
    }

    /// <summary>Opens the settings</summary>
    public void OpenSettings() => MenuManager.OpenOverlay(SettingsMenu);
}
