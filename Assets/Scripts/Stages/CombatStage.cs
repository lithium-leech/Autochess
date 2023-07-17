using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// The stage of the game where the pieces fight
/// </summary>
public class CombatStage : IStage
{
    /// <summary>Creates a new instance of a CombatStage</summary>
    /// <param name="game">The Game to run the CombatStage in</param>
    public CombatStage(Game game)
    {
        Game = game;
    }

    /// <summary>The Game this is being run in</summary>
    private Game Game { get; set; }

    /// <summary>The time elapsed so far, in between turns</summary>
    private float TimeWaited { get; set; }

    /// <summary>True if it is currently the player's turn</summary>
    private bool IsPlayerTurn { get; set; }
    
    /// <summary>True until the first turn has been taken</summary>
    private bool IsFirstTurn { get; set; } = true;

    /// <summary>The number of rounds until the player should be prompted to concede</summary>
    private int RoundsToConcede { get; set; }

    /// <summary>The number of rounds that have occurred with no pieces moving</summary>
    private int RoundsStatic { get; set; }

    /// <summary>True if the concede menu is currently being shown</summary>
    private bool ShowingConcede { get; set; }

    public void Start()
    {
        // Initialize battle start
        GameState.MusicBox.StopMusic();
        TimeWaited = 0;
        IsPlayerTurn = GameState.IsPlayerWhite;
        EndConcede();

        // Log starting board state
        StringBuilder positions = new StringBuilder("(");
        for (int x = 0; x < Game.GameBoard.Width; x++)
        for (int y = 0; y < Game.GameBoard.Height; y++)
        {
            Space space = Game.GameBoard.GetSpace(new Vector2Int(x, y));
            if (space !=  null)
            {
                string msg = space.LogDisplay();
                if (!string.IsNullOrEmpty(msg)) positions.Append(msg);
            }
        }
        positions.Append(" )");
        Debug.Log($"Starting Positions: {positions}");
    }

    private string ControlString(Piece piece) => piece.IsPlayer ? "Player" : "Enemy";

    public void During()
    {
        // Pause between turns
        TimeWaited += Time.deltaTime;
        if (TimeWaited < GameState.TurnPause) return;
        if (Game.GameBoard.ArePiecesMoving()) return;
        
        // Start the fight music before the first move is taken
        if (IsFirstTurn)
        {
            IsFirstTurn = false;
            GameState.MusicBox.PlayMusic(SongName.Battle);
            TimeWaited = GameState.TurnPause - 0.15f;
            return;
        }

        // Check if the battle is over
        if (Game.GameBoard.PlayerPieces.Count < 1)
        {
            // The player lost if they have no pieces left
            Game.NextStage = new DefeatStage(Game);
            return;
        }
        else if (Game.GameBoard.EnemyPieces.Count < 1)
        {
            // The player won if there are no enemy pieces left
            Game.NextStage = new UpgradeStage(Game);
            return;
        }
        
        // Move the current player's pieces
        List<Piece> pieces;
        string actor;
        if (IsPlayerTurn)
        {
            pieces = new List<Piece>(Game.GameBoard.PlayerPieces);
            actor = "Player";
        }
        else
        {
            pieces = new List<Piece>(Game.GameBoard.EnemyPieces);
            actor = "Enemy";
        }
        GameState.IsActiveRound = false;
        RunRound(pieces);
        if (GameState.IsActiveRound) RoundsStatic = 0;
        else RoundsStatic++;

        // Log the moves
        StringBuilder moves = new StringBuilder("(");
        foreach (Piece piece in pieces)
            moves.Append($" {piece.Kind}[{piece.Space.X},{piece.Space.Y}]");
        moves.Append(" )");
        Debug.Log($"{actor} Moves: {moves}");

        // Check if the player should be prompted to concede
        if (!ShowingConcede)
            if (RoundsStatic > 1 || RoundsToConcede < 1)
                StartConcede();

        // Go to the next turn
        RoundsToConcede--;
        IsPlayerTurn = !IsPlayerTurn;
        TimeWaited = 0;
    }

    public void End()
    {
        // Remove any remaining pieces
        Game.GameBoard.Clear();
        Game.SideBoard.Clear();

        // Remove the concede menu
        EndConcede();
    }

    /// <summary>Runs the battle operations for one set of pieces</summary>
    /// <param name="pieces">The pieces to move</param>
    private void RunRound(List<Piece> pieces)
    {
        foreach (Piece piece in pieces) piece.TakeTurn();
    }

    /// <summary>Displays the concede menu to the player</summary>
    private void StartConcede()
    {
        ShowingConcede = true;
        MenuManager.AddActiveMenu(Game.ConcedeMenu);
        Game.CancelConcedeButton.onClick.AddListener(EndConcede);
        Game.ConfirmConcedeButton.onClick.AddListener(ConfirmConcede);
    }

    /// <summary>The player concedes and the fight is lost</summary>
    private void ConfirmConcede()
    {
        Game.NextStage = new DefeatStage(Game);
        EndConcede();
    }

    /// <summary>Removes the concede menu and resets concede logic</summary>
    private void EndConcede()
    {
        MenuManager.RemoveActiveMenu(Game.ConcedeMenu);
        Game.CancelConcedeButton.onClick.RemoveAllListeners();
        Game.ConfirmConcedeButton.onClick.RemoveAllListeners();
        RoundsStatic = 0;
        RoundsToConcede = 40;
        ShowingConcede = false;
    }
}
