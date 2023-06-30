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

    /// <summary>The enemies roster of pieces (indexes to prefabs)</summary>
    public IList<AssetGroup.Piece> EnemyPieces { get; set; }

    /// <summary>The player's pieces on the game board</summary>
    public IList<PositionRecord> PlayerGameBoard { get; set; }

    /// <summary>The player's pieces on the side board</summary>
    public IList<PositionRecord> PlayerSideBoard { get; set; }

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
        EnemyPieces = new List<AssetGroup.Piece>();
        PlayerGameBoard = new List<PositionRecord>();
        PlayerSideBoard = new List<PositionRecord>();
        EnemyPieces.Add(AssetGroup.Piece.Pawn);
        PlayerSideBoard.Add(new PositionRecord(AssetGroup.Piece.Pawn, null));

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

    /// <summary>Creates a new instance of a piece</summary>
    /// <param name="kind">The kind of piece to create</param>
    /// <param name="player">True if the piece is for the player</param>
    /// <param name="white">True if the piece is white</param>
    /// <returns>A Piece</returns>
    public Piece CreatePiece(AssetGroup.Piece kind, bool player, bool white)
    {
        AssetGroup.Group groupKey = white ? AssetGroup.Group.WhitePiece : AssetGroup.Group.BlackPiece;
        GameObject obj = Instantiate(AssetManager.Prefabs[groupKey][(int)kind]);
        Piece piece = obj.GetComponent<Piece>();
        piece.IsPlayer = player;
        piece.IsWhite = white;
        return piece;
    }

    /// <summary>Creates a new instance of a power</summary>
    /// <param name="kind">The kind of power to create</param>
    /// <param name="player">True if the power is for the player</param>
    /// <returns>A Power</returns>
    public Power CreatePower(AssetGroup.Power kind, bool player)
    {
        GameObject obj = Instantiate(AssetManager.Prefabs[AssetGroup.Group.Power][(int)kind]);
        Power power = obj.GetComponent<Power>();
        power.Game = this;
        power.IsPlayer = player;
        return power;
    }

    /// <summary>Creates a new instance of a panel</summary>
    /// <param name="kind">The kind of panel to create</param>
    /// <returns>A Panel</returns>
    public GameObject CreatePanel(bool white, bool info)
    {
        AssetGroup.Panel kind;
        if (white)
            if (info) kind = AssetGroup.Panel.WhiteInfo;
            else kind = AssetGroup.Panel.WhiteTile;
        else
            if (info) kind = AssetGroup.Panel.BlackInfo;
            else kind = AssetGroup.Panel.BlackTile;
        GameObject panel = Instantiate(AssetManager.Prefabs[AssetGroup.Group.Panel][(int)kind]);
        return panel;
    }

    /// <summary>Creates a new instance of a single highlight object</summary>
    /// <param name="kind">The kind of highlight to create</param>
    /// <param name="board">The board to load the highlight on</param>
    /// <param name="space">The space to place the highlight on</param>
    /// <returns>A new highlight object</returns>
    public GameObject CreateHighlight(AssetGroup.Highlight kind, Board board, Vector2Int space)
    {
        AssetGroup.Group groupKey = AssetGroup.Group.Highlight;
        GameObject highlight = Instantiate(AssetManager.Prefabs[groupKey][(int)kind]);
        highlight.transform.position = board.ToPosition(space);
        return highlight;
    }
}
