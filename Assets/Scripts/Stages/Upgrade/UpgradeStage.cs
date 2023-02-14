using System;
using TMPro;

/// <summary>
/// The stage of the game where the player chooses upgrades for the next level
/// </summary>
public class UpgradeStage : IStage
{
    /// <summary>Creates a new instance of an UpgradeStage</summary>
    /// <param name="game">The Game to run the UpgradeStage in</param>
    public UpgradeStage(Game game)
    {
        Game = game;
    }

    /// <summary>The Game this is being run in</summary>
    private Game Game { get; set; }

    /// <summary>The next level choices the player is being offered</summary>
    public NextLevelChoices Choices { get; set; }

    public void Start()
    {
        // Start the planning music
        Game.MusicBox.PlayMusic(0);

        // Go to the next level
        Game.Level++;
        if (Game.Level > Game.HighScore) Game.HighScore = Game.Level;
        Game.LevelText.GetComponent<TextMeshProUGUI>().text = Game.Level.ToString();

        // Display new level choices
        Choices = new NextLevelChoices();
        Choices.ShowPiecesInMenu(Game);
        Game.UpgradeMenu.SetActive(true);

        // Delete any pieces in the trash
        foreach (Piece piece in Game.TrashBoard.PlayerPieces)
            piece.IsCaptured= true;

        // Add button listeners
        Game.ChoiceOneButton.onClick.AddListener(ChooseOne);
        Game.ChoiceTwoButton.onClick.AddListener(ChooseTwo);
        Game.ChoiceThreeButton.onClick.AddListener(ChooseThree);
    }

    public void During()
    {

    }

    public void End()
    {
        
    }

    // Button listeners
    public void ChooseOne() => Choose(1);
    public void ChooseTwo() => Choose(2);
    public void ChooseThree() => Choose(3);

    /// <summary>Applies the changes for the next level option selected by the player</summary>
    /// <param name="choice">The option number selected by the player</param>
    private void Choose(int choice)
    {
        // Get the pieces corresponding to the selected option
        Type playerPiece;
        Type enemyPiece;
        if (choice == 1)
        {
            playerPiece = Choices.PlayerPiece1;
            enemyPiece = Choices.EnemyPiece1;
        }
        else if (choice == 2)
        {
            playerPiece = Choices.PlayerPiece2;
            enemyPiece = Choices.EnemyPiece2;
        }
        else if (choice == 3)
        {
            playerPiece = Choices.PlayerPiece3;
            enemyPiece = Choices.EnemyPiece3;
        }
        else
        {
            throw new Exception($"Next level choice {choice} not recognized");
        }

        // Add the enemy piece, removing a lower value piece if necessary
        if (Game.EnemyPieces.Count == 16) RemoveLowerValue(enemyPiece);
        if (Game.EnemyPieces.Count < 16) Game.EnemyPieces.Add(enemyPiece);

        // Add the player piece as long a sideboard space is empty
        if (Game.PlayerSideBoard.Count < 16) Game.PlayerSideBoard.Add(new PositionRecord(playerPiece, null));

        // Remove the upgrade menu
        Choices.RemovePiecesInMenu();
        Game.UpgradeMenu.SetActive(false);
        Game.ChoiceOneButton.onClick.RemoveAllListeners();
        Game.ChoiceTwoButton.onClick.RemoveAllListeners();
        Game.ChoiceThreeButton.onClick.RemoveAllListeners();

        // Go to the planning phase
        Game.NextStage = new PlanningStage(Game);
    }

    /// <summary>Remove an enemy piece with lower value than the given Type</summary>
    /// <param name="type">The piece Type to make space for</param>
    public void RemoveLowerValue(Type type)
    {
        int valueToAdd = GetTypeValue(type);
        for (int i = 0; i < Game.EnemyPieces.Count; i++)
        {
            int valueToRemove = GetTypeValue(Game.EnemyPieces[i]);
            if (valueToRemove <= valueToAdd)
            {
                Game.EnemyPieces.RemoveAt(i);
                return;
            }
        }
    }

    /// <summary>Get the value of a give piece Type</summary>
    /// <param name="type">The piece Type to evaluate</param>
    /// <returns>An integer value</returns>
    public int GetTypeValue(Type type)
    {
        if (type == typeof(Pawn)) return 0;
        else if (type == typeof(Knight)) return 1;
        else if (type == typeof(King)) return 2;
        else if (type == typeof(Bishop)) return 3;
        else if (type == typeof(Rook)) return 4;
        else if (type == typeof(Queen)) return 5;
        else throw new Exception($"Type {type} not recognized");
    }
}
