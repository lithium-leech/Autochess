using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

/// <summary>
/// The main game object, responsible for managing
/// the state of the game
/// </summary>
public class Game : MonoBehaviour
{
    /// Properties to set using Unity interface
    public TextMeshProUGUI LevelText;
    public Button FightButton;
    public Button CancelConcedeButton;
    public Button ConfirmConcedeButton;
    public RewardedAdsButton RetryButton;
    public Button EndGameButton;
    public Button ChoiceOneButton;
    public Button ChoiceTwoButton;
    public Button ChoiceThreeButton;
    public GameObject InGameMenu;
    public GameObject ConcedeMenu;
    public GameObject UpgradeMenu;
    public GameObject GameOverMenu;
    public GameObject SettingsMenu;

    /// <summary>The current stage of the game being run</summary>
    public IStage CurrentStage { get; set; } = null;

    /// <summary>The next stage of the game to run</summary>
    /// <remarks>null when the current stage should keep runnning</remarks>
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

    /// <summary>The board for discarding unwanted peices</summary>
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
        GameBoard = new Board(this, 8, 8, 2, new Vector2(-4.0f, -1.0f));
        SideBoard = new Board(this, 8, 2, 2, new Vector2(-4.0f, -4.5f));
        TrashBoard = new Board(this, 1, 1, 1, new Vector2(-3.0f, -7.0f));

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
    /// <param name="white">True if the piece is white</param>
    /// <returns>A Piece</returns>
    public Piece CreatePiece(AssetGroup.Piece kind, bool white)
    {
        AssetGroup.Groups groupKey = white ? AssetGroup.Groups.WhitePiece : AssetGroup.Groups.BlackPiece;
        GameObject obj = Instantiate(AssetManager.Prefabs[groupKey][(int)kind]);
        Piece piece = obj.GetComponent<Piece>();
        piece.IsPlayerPiece = !white;
        return piece;
    }

    /// <summary>Creates a new instance of a single highlight object</summary>
    /// <param name="kind">The kind of highlight to create</param>
    /// <param name="board">The board to load the highlight on</param>
    /// <param name="space">The space to place the highlight on</param>
    /// <returns>A new highlight object</returns>
    public GameObject CreateHighlight(AssetGroup.Highlight kind, Board board, Vector2Int space)
    {
        AssetGroup.Groups groupKey = AssetGroup.Groups.Highlight;
        GameObject highlight = Instantiate(AssetManager.Prefabs[groupKey][(int)kind]);
        highlight.transform.position = board.ToPosition(space);
        return highlight;
    }
}
