using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    public GameObject GameOverMenu;
    public GameObject ConcedeMenu;
    public GameObject UpgradeMenu;

    /// <summary>The index of the music currently being played</summary>
    private int CurrentMusic { get; set; } = -1;
    
    /// <summary>The time elapsed so far, in between turns</summary>
    private float TimeWaited { get; set; }

    /// <summary>True if it is currently white's turn</summary>
    private bool WhiteTurn { get; set; }
    
    /// <summary>The Piece currently being moved around by the player</summary>
    private Piece HeldPiece { get; set; }

    /// <summary>Space highlights displayed during the planning phase</summary>
    private IEnumerable<GameObject> Highlights { get; set; }

    /// <summary>The enemies roster of pieces (indexes to prefabs)</summary>
    private IList<Type> EnemyPieces { get; set; }

    /// <summary>The player's roster of pieces</summary>
    private IList<Type> PlayerPieces { get; set; }

    /// <summary>The next level choices the player is currently being offered</summary>
    private NextLevelChoices CurrentChoices { get; set; }

    private void Start()
    {
        // Load saved data
        SaveData saveData = SaveSystem.Load();

        // Set game states
        GameState.PlanningStarted = false;
        GameState.InPlanning = false;
        GameState.PlanningOver = false;
        GameState.FightStarted = false;
        GameState.InFight = false;
        GameState.FightOver = false;
        GameState.Victory = false;
        GameState.ConcedeStarted = false;
        GameState.InConcede = false;
        GameState.Level = 1;
        GameState.HighScore = saveData.HighScore;
        GameState.RoundsToConcede = 20;
        GameState.RoundsStatic = 0;
        LevelText.GetComponent<TextMeshProUGUI>().text = "1";

        // Create the game boards
        GameState.GameBoard = new Board(this, 8, 8, 2, new Vector2(-4.0f, 1.0f));
        GameState.SideBoard = new Board(this, 8, 3, 3, new Vector2(-4.0f, -4.0f));

        // Create a sample setup;
        EnemyPieces = new List<Type>();
        PlayerPieces = new List<Type>();
        EnemyPieces.Add(typeof(Pawn));
        PlayerPieces.Add(typeof(Pawn));

        // Start the planning phase
        GameState.PlanningStarted = true;
    }

    private void Update()
    {
        if (!GameState.InPlanning && GameState.PlanningStarted) DetectPlanningStart();
        if (GameState.InPlanning) RunPlanning();
        if (GameState.InPlanning && GameState.PlanningOver) DetectPlanningFinish();
        if (!GameState.InFight && GameState.FightStarted) DetectFightStart();
        if (GameState.InFight) RunFight();
        if (GameState.InFight && GameState.FightOver) DetectFightFinish();
        if (!GameState.InConcede && GameState.ConcedeStarted) PromptConcede();
    }

    private void OnDestroy()
    {
        // Create the save data
        SaveData saveData = new SaveData();
        saveData.HighScore = GameState.HighScore;

        // Write the save data to a file
        SaveSystem.Save(saveData);
    }

    /// <summary>Starts the planning phase</summary>
    private void DetectPlanningStart()
    {
        // Only run the planning started sequence once
        GameState.PlanningStarted = false;
        GameState.InPlanning = true;

        // Start the planning music
        PlayMusic(0);

        // Display new level choices (unless this is the first level)
        if (GameState.Level > 1)
        {
            CurrentChoices = new NextLevelChoices();
            CurrentChoices.ShowPiecesInMenu(this);
            UpgradeMenu.SetActive(true);
        }
        else PostChoiceSetup();
    }

    /// <summary>Applies the changes for the next level options selected by the player</summary>
    /// <param name="choice">The option number selected by the player</param>
    public void PlanningChoice(int choice)
    {
        CurrentChoices.RemovePiecesInMenu();
        UpgradeMenu.SetActive(false);
        Type playerPiece;
        Type enemyPiece;
        if (choice == 1)
        {
            playerPiece = CurrentChoices.PlayerPiece1;
            enemyPiece = CurrentChoices.EnemyPiece1;
        }
        else if (choice == 2)
        {
            playerPiece = CurrentChoices.PlayerPiece2;
            enemyPiece = CurrentChoices.EnemyPiece2;
        }
        else if (choice == 3)
        {
            playerPiece = CurrentChoices.PlayerPiece3;
            enemyPiece = CurrentChoices.EnemyPiece3;
        }
        else
        {
            throw new Exception($"Next level choice {choice} not recognized");
        }
        if (EnemyPieces.Count < 16) EnemyPieces.Add(enemyPiece);
        if (PlayerPieces.Count < 24) PlayerPieces.Add(playerPiece);
        PostChoiceSetup();
    }

    /// <summary>Sets up the game boards after the player makes their selection for the next level</summary>
    private void PostChoiceSetup()
    {
        // Set up the boards
        PlaceEnemyPieces();
        PlacePlayerPieces();

        // Add highlights around spaces that the player can put pieces
        List<GameObject> highlights = new();
        highlights.AddRange(AddHighlights(GameState.GameBoard));
        highlights.AddRange(AddHighlights(GameState.SideBoard));
        Highlights = highlights;
    }

    /// <summary>Handles player inputs during the planning phase</summary>
    private void RunPlanning()
    {
        // Get the current mouse position
        Vector3 position = Camera.ScreenToWorldPoint(Input.mousePosition);
        position.z = GameState.PieceZ;

        // Check if the position is inside a board
        Board board = null;
        if (GameState.GameBoard.CornerBL.x <= position.x &&
            position.x <= GameState.GameBoard.CornerTR.x &&
            GameState.GameBoard.CornerBL.y <= position.y &&
            position.y <= GameState.GameBoard.CornerTR.y)
            board = GameState.GameBoard;
        else if (GameState.SideBoard.CornerBL.x <= position.x &&
            position.x <= GameState.SideBoard.CornerTR.x &&
            GameState.SideBoard.CornerBL.y <= position.y &&
            position.y <= GameState.SideBoard.CornerTR.y)
            board = GameState.SideBoard;
        
        // If it is inside a board, get the space/piece
        Vector2Int space = new(-1, -1);
        if (board != null) space = board.ToSpace(position);

        // Pick up a piece when the screen is clicked/pressed
        if (Input.GetMouseButtonDown(0) && board != null)
        {
            HeldPiece = board.Spaces[space.x, space.y];
            if (HeldPiece != null)
                if (HeldPiece.IsPlayerPiece)
                {
                    board.RemovePiece(HeldPiece);
                    HeldPiece.transform.position = position - (Vector3.forward * 10);
                }
                else HeldPiece = null;
        }
        
        // Drag the selected piece as the pointer moves
        else if (Input.GetMouseButton(0) && HeldPiece != null)
        {
            HeldPiece.transform.position = position - (Vector3.forward * 10);
        }
        
        // Drop the piece in a new space (Or the old one)
        else if (Input.GetMouseButtonUp(0) && HeldPiece != null)
        {
            if (board != null && board.IsPlayerSpace(space) && !board.HasPiece(space)) board.AddPiece(HeldPiece, space);
            else HeldPiece.Board.AddPiece(HeldPiece, HeldPiece.Space);
            HeldPiece = null;
        }
    }

    /// <summary>Performs end of planning phase operations</summary>
    private void DetectPlanningFinish()
    {
        // Only run the fight over sequence once
        GameState.PlanningOver = false;
        GameState.InPlanning = false;

        // Stop the planning music
        StopMusic();

        // Remove highlights loaded in planning start
        foreach (GameObject highlight in Highlights) Destroy(highlight);
        Highlights = new List<GameObject>();
    }

    /// <summary>Starts a fight once it's been switched on</summary>
    private void DetectFightStart()
    {
        // Only run the fight started sequence once
        GameState.FightStarted = false;
        GameState.InFight = true;
        GameState.Victory = false;
        GameState.RoundsToConcede = 20;
        GameState.RoundsStatic = 0;

        // Set the initial fighting states
        TimeWaited = 0;
        WhiteTurn = true;
    }

    /// <summary>Runs the battle operations</summary>
    private void RunFight()
    {
        // Pause between turns
        TimeWaited += Time.deltaTime;
        if (TimeWaited < GameState.TurnPause) return;

        // Start the fight music when the first move is taken
        if (CurrentMusic < 0) PlayMusic(1);

        // Check if the battle is over
        if (GameState.GameBoard.PlayerPieces.Count < 1)
        {
            GameState.FightOver = true;
            return;
        }
        else if (GameState.GameBoard.EnemyPieces.Count < 1)
        {
            GameState.Victory = true;
            GameState.FightOver = true;
            return;
        }

        // Move the current player's pieces
        if (WhiteTurn) RunRound(GameState.GameBoard.EnemyPieces);
        else RunRound(GameState.GameBoard.PlayerPieces);

        // Check if the player should be prompted to concede
        if (!GameState.InConcede)
            if (GameState.RoundsStatic > 1 || GameState.RoundsToConcede < 1)
                GameState.ConcedeStarted = true;

        // Go to the next turn
        GameState.RoundsToConcede--;
        WhiteTurn = !WhiteTurn;
        TimeWaited = 0;
    }

    /// <summary>Runs the battle operations for one set of pieces</summary>
    /// <param name="pieces">The pieces to move</param>
    private void RunRound(List<Piece> pieces)
    {
        bool pieceMoved = false;
        foreach (Piece piece in pieces)
        {
            if (pieceMoved) piece.TakeTurn();
            else
            {
                Vector2Int space = piece.Space;
                piece.TakeTurn();
                if (space != piece.Space) pieceMoved = true;
            }
        }
        if (!pieceMoved) GameState.RoundsStatic++;
    }

    /// <summary>Performs end of fight operations</summary>
    private void DetectFightFinish()
    {
        // Only run the fight over sequence once
        GameState.FightOver = false;
        GameState.InFight = false;
        GameState.ConcedeStarted = false;
        GameState.InConcede = false;
        ConcedeMenu.SetActive(false);

        // Stop the battle music
        StopMusic();

        // Determine if the battle was won
        if (GameState.Victory)
        {
            GameState.Level++;
            if (GameState.Level > GameState.HighScore) GameState.HighScore = GameState.Level;
            LevelText.GetComponent<TextMeshProUGUI>().text = GameState.Level.ToString();
            GameState.PlanningStarted = true;
        }
        else
        {
            // Load the Game Over menu
            ScoreText.GetComponent<TextMeshProUGUI>().text = GameState.Level.ToString();
            HighScoreText.GetComponent<TextMeshProUGUI>().text = GameState.HighScore.ToString();
            GameOverMenu.SetActive(true);
        }
    }

    /// <summary>Shows a prompt asking the player if they want to concede</summary>
    private void PromptConcede()
    {
        ConcedeMenu.SetActive(true);
    }

    /// <summary>The player concedes defeat and the fight is lost</summary>
    public void ConfirmConcede()
    {
        ConcedeMenu.SetActive(false);
        GameState.ConcedeStarted = false;
        GameState.InConcede = false;
        GameState.FightOver = true;
    }

    /// <summary>The player does not concede and the fight continues</summary>
    public void CancelConcede()
    {
        ConcedeMenu.SetActive(false);
        GameState.ConcedeStarted = false;
        GameState.InConcede = false;
        GameState.RoundsStatic = 0;
        GameState.RoundsToConcede = 20;
    }

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
        piece.Board = GameState.GameBoard;
        return piece;
    }

    /// <summary>Add highlights around the player rows of the given board</summary>
    /// <param name="board">The board to add highlights to</param>
    /// <returns>The created highlight objects</returns>
    private IEnumerable<GameObject> AddHighlights(Board board)
    {
        // Start with an empty list
        IList<GameObject> highlights = new List<GameObject>();

        // No highlights needed if there are 0 player rows
        if (board.PlayerRows > 0)
        {
            // Create the bottom row
            highlights.Add(CreateHighlight(0, board, new Vector2Int(-1, -1)));
            for (int i = 0; i < board.Width; i++) highlights.Add(CreateHighlight(7, board, new Vector2Int(i, -1)));
            highlights.Add(CreateHighlight(6, board, new Vector2Int(board.Width, -1)));

            // Create the left and right columns
            for (int i = 0; i < board.PlayerRows; i++)
            {
                highlights.Add(CreateHighlight(5, board, new Vector2Int(-1, i)));
                highlights.Add(CreateHighlight(1, board, new Vector2Int(board.Width, i)));
            }

            // Create the top row
            highlights.Add(CreateHighlight(2, board, new Vector2Int(-1, board.PlayerRows)));
            for (int i = 0; i < board.Width; i++) highlights.Add(CreateHighlight(3, board, new Vector2Int(i, board.PlayerRows)));
            highlights.Add(CreateHighlight(4, board, new Vector2Int(board.Width, board.PlayerRows)));
        }

        // Return the generated highlights
        return highlights;
    }

    /// <summary>Creates a single space highlight object</summary>
    /// <param name="prefab">The prefab index to load a highlight for</param>
    /// <param name="board">The board to load the highlight on</param>
    /// <param name="space">The space to place the highlight on</param>
    /// <returns>A new highlight object</returns>
    private GameObject CreateHighlight(int prefab, Board board, Vector2Int space)
    {
        GameObject highlight = Instantiate(HighlightPrefabs[prefab]);
        highlight.transform.position = board.ToPosition(space);
        return highlight;
    }

    /// <summary>Sets up the enemy's roster of pieces on their side of the board</summary>
    private void PlaceEnemyPieces()
    {
        GameState.GameBoard.Clear();
        foreach (Type pieceType in EnemyPieces)
        {
            Vector2Int space = GetRandomEmptySpace();
            GameState.GameBoard.AddPiece(pieceType, true, space);
        }
    }

    /// <summary>Sets up the player's roster of pieces in the sideboard</summary>
    private void PlacePlayerPieces()
    {
        GameState.SideBoard.Clear();
        foreach (Type pieceType in PlayerPieces) GameState.SideBoard.AddPiece(pieceType, false);
    }

    /// <summary>Gets a random empty space on the enemy's side of the game board</summary>
    /// <returns>An empty space on the game board</returns>
    private Vector2Int GetRandomEmptySpace()
    {
        IList<Vector2Int> emptySpaces = new List<Vector2Int>();
        for (int x = 0; x < GameState.GameBoard.Width; x++)
        for (int y = 6; y < GameState.GameBoard.Height; y++)
        {
            Vector2Int space = new(x, y);
            if (!GameState.GameBoard.HasPiece(space)) emptySpaces.Add(space);
        }
        return emptySpaces[UnityEngine.Random.Range(0, emptySpaces.Count - 1)];
    }

    /// <summary>Plays the music at the given index</summary>
    /// <param name="index">The index of the music to play</param>
    private void PlayMusic(int index)
    {
        StopMusic();
        CurrentMusic = index;
        Music[CurrentMusic].Play();
    }
    
    /// <summary>Stops any music that's playing</summary>
    private void StopMusic()
    {
        if (CurrentMusic > -1)
        {
            Music[CurrentMusic].Stop();
            CurrentMusic = -1;
        }
    }
}
