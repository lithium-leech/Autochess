using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The main game object, responsible for managing
/// the state of the game
/// </summary>
public class Game : MonoBehaviour
{
    /// Properties to set using Unity interface
    public AudioSource[] Music;
    public GameObject[] PiecePrefabs;
    public GameObject[] HighlightPrefabs;
    public Camera Camera;
    public GameObject LevelText;
    public GameObject ScoreText;
    public GameObject HighScoreText;
    public GameObject ExitGameButton;
    public Button FightButton;
    public Button CancelConcedeButton;
    public Button ConfirmConcedeButton;
    public RewardedAdsButton RetryButton;
    public Button StartOverButton;
    public Button ChoiceOneButton;
    public Button ChoiceTwoButton;
    public Button ChoiceThreeButton;
    public GameObject GameOverMenu;
    public GameObject ConcedeMenu;
    public GameObject UpgradeMenu;

    /// <summary>The time in between turns (seconds)</summary>
    public static float TurnPause { get; } = 2;

    /// <summary>The Z-plane that pieces exist on</summary>
    public static float PieceZ { get; } = -1;

    /// <summary>The current stage of the game being run</summary>
    public IStage CurrentStage { get; set; } = null;

    /// <summary>The next stage of the game to run</summary>
    /// <remarks>null when the current stage should keep runnning</remarks>
    public IStage NextStage { get; set; } = null;

    /// <summary>An object for playing music</summary>
    public MusicBox MusicBox { get; set; }

    /// <summary>The enemies roster of pieces (indexes to prefabs)</summary>
    public IList<Type> EnemyPieces { get; set; }

    /// <summary>The player's pieces on the game board</summary>
    public IList<PositionRecord> PlayerGameBoard { get; set; }

    /// <summary>The player's pieces on the side board</summary>
    public IList<PositionRecord> PlayerSideBoard { get; set; }

    /// <summary>The current level</summary>
    public int Level { get; set; } = 0;

    /// <summary>The player's highest level achieved</summary>
    public int HighScore { get; set; } = 0;

    /// <summary>The board that fights take place on</summary>
    public Board GameBoard { get; set; }

    /// <summary>The board that holds the player's available pieces</summary>
    public Board SideBoard { get; set; }

    private void Start()
    {
        // Load saved data
        SaveData saveData = SaveSystem.Load();

        // Set game states
        MusicBox = new MusicBox(Music);
        Level = 1;
        HighScore = saveData.HighScore;
        LevelText.GetComponent<TextMeshProUGUI>().text = "1";

        // Create the game boards
        GameBoard = new Board(this, 8, 8, 2, new Vector2(-4.0f, -1.0f));
        SideBoard = new Board(this, 8, 2, 2, new Vector2(-4.0f, -5.0f));

        // Create a sample setup;
        EnemyPieces = new List<Type>();
        PlayerGameBoard = new List<PositionRecord>();
        PlayerSideBoard = new List<PositionRecord>();
        EnemyPieces.Add(typeof(Pawn));
        PlayerSideBoard.Add(new PositionRecord(typeof(Pawn), new Vector2Int(0,1)));

        // Start the planning phase
        NextStage = new PlanningStage(this);
    }

    private void Update()
    {
        // Check if a new stage has been queued
        if (NextStage != null)
        {
            // Replace the current stage with the next one
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

    private void OnDestroy()
    {
        // Create the save data
        SaveData saveData = new SaveData();
        saveData.HighScore = HighScore;

        // Write the save data to a file
        SaveSystem.Save(saveData);
    }

    /// <summary>Exits the game and goes back to the main menu</summary>
    public void ExitGame() => SceneManager.LoadScene("Main Menu");

    /// <summary>Creates a new instance of a piece</summary>
    /// <typeparam name="T">The type of piece to create</typeparam>
    /// <param name="white">True if the piece is white</param>
    /// <returns>A Piece</returns>
    public Piece CreatePiece<T>(bool white) where T : Piece => CreatePiece(typeof(T), white);

    /// <summary>Creates a new instance of a piece</summary>
    /// <param name="type">The type of piece to create</param>
    /// <param name="white">True if the piece is white</param>
    /// <returns>A Piece</returns>
    public Piece CreatePiece(Type type, bool white)
    {
        int prefab;
        if (type == typeof(King)) prefab = 0;
        else if (type == typeof(Queen)) prefab = 2;
        else if (type == typeof(Bishop)) prefab = 4;
        else if (type == typeof(Knight)) prefab = 6;
        else if (type == typeof(Rook)) prefab = 8;
        else if (type == typeof(Pawn)) prefab = 10;
        else throw new Exception($"Piece Type {type} not recognized");
        if (!white) prefab++;
        GameObject obj = Instantiate(PiecePrefabs[prefab]);
        Piece piece = (Piece) obj.GetComponent(type);
        piece.IsPlayerPiece = !white;
        piece.Board = GameBoard;
        return piece;
    }

    /// <summary>Creates a single space highlight object</summary>
    /// <param name="prefab">The prefab index to load a highlight for</param>
    /// <param name="board">The board to load the highlight on</param>
    /// <param name="space">The space to place the highlight on</param>
    /// <returns>A new highlight object</returns>
    public GameObject CreateHighlight(int prefab, Board board, Vector2Int space)
    {
        GameObject highlight = Instantiate(HighlightPrefabs[prefab]);
        highlight.transform.position = board.ToPosition(space);
        return highlight;
    }
}
