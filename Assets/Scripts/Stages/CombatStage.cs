using System.Collections.Generic;
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
    public float TimeWaited { get; set; }

    /// <summary>True if it is currently white's turn</summary>
    public bool WhiteTurn { get; set; }

    /// <summary>The number of rounds until the player should be prompted to concede</summary>
    public int RoundsToConcede { get; set; }

    /// <summary>The number of rounds that have occured with no pieces moving</summary>
    public int RoundsStatic { get; set; }

    /// <summary>True if the concede menu is currently being shown</summary>
    public bool ShowingConcede { get; set; }

    public void Start()
    {
        Game.MusicBox.StopMusic();
        TimeWaited = 0;
        WhiteTurn = true;
        EndConcede();
    }

    public void During()
    {
        // Pause between turns
        TimeWaited += Time.deltaTime;
        if (TimeWaited < Game.TurnPause) return;

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
        
        // Start the fight music when the first move is taken
        Game.MusicBox.PlayMusic(1);

        // Move the current player's pieces
        if (WhiteTurn) RunRound(Game.GameBoard.EnemyPieces);
        else RunRound(Game.GameBoard.PlayerPieces);

        // Check if the player should be prompted to concede
        if (!ShowingConcede)
            if (RoundsStatic > 1 || RoundsToConcede < 1)
                StartConcede();

        // Go to the next turn
        RoundsToConcede--;
        WhiteTurn = !WhiteTurn;
        TimeWaited = 0;
    }

    public void End()
    {
        // Remove the concede menu
        EndConcede();
    }

    /// <summary>Runs the battle operations for one set of pieces</summary>
    /// <param name="pieces">The pieces to move</param>
    public void RunRound(List<Piece> pieces)
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
        if (pieceMoved) RoundsStatic = 0;
        else RoundsStatic++;
    }

    /// <summary>Displays the concede menu to the player</summary>
    public void StartConcede()
    {
        ShowingConcede = true;
        Game.ConcedeMenu.SetActive(true);
        Game.CancelConcedeButton.onClick.AddListener(EndConcede);
        Game.ConfirmConcedeButton.onClick.AddListener(ConfirmConcede);
    }

    /// <summary>The player concedes and the fight is lost</summary>
    public void ConfirmConcede()
    {
        Game.NextStage = new DefeatStage(Game);
        EndConcede();
    }

    /// <summary>Removes the concede menu and resets concede logic</summary>
    public void EndConcede()
    {
        Game.ConcedeMenu.SetActive(false);
        Game.CancelConcedeButton.onClick.RemoveAllListeners();
        Game.ConfirmConcedeButton.onClick.RemoveAllListeners();
        RoundsStatic = 0;
        RoundsToConcede = 40;
        ShowingConcede = false;
    }
}
