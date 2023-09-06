using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// Holds information about the choices a player has when
/// they're choosing pieces for the next level
/// </summary>
public class PieceChoices : UpgradeChoices
{
    /// <summary>Creates a new set of piece choices</summary>
    public PieceChoices(Game game) : base(game)
    {
        // Pick random pieces
        PlayerPieces = new AssetGroup.Piece[NumberOfChoices];
        EnemyPieces = new AssetGroup.Piece[NumberOfChoices];
        for (int i = 0; i < NumberOfChoices; i++)
        {
            PiecePair choice = Game.CurrentSet.GetPieceChoice();
            PlayerPieces[i] = choice.Player;
            EnemyPieces[i] = choice.Enemy;
        }
    }

    public override string TitleText { get; } = "ChoosePiece";

    /// <summary>The available powers for the player</summary>
    private AssetGroup.Piece[] PlayerPieces { get; }

    /// <summary>The available powers for the enemy</summary>
    private AssetGroup.Piece[] EnemyPieces { get; }

    protected override void ShowChoices()
    {
        for (int i = 0; i < NumberOfChoices; i++)
        {
            CreatePiece(i, true, false);
            CreatePiece(i, false, false);
        }
    }

    public override void ShowInfo(int choice)
    {
        // Erase previous info
        RemoveInfo();

        // Create new sprites
        Piece playerPiece = CreatePiece(choice, true, true);
        Piece enemyPiece = CreatePiece(choice, false, true);

        // Change text
        Game.PlayerChoiceNameText.StringReference = new LocalizedString("PieceNames", $"{playerPiece.Kind}");
        Game.PlayerChoiceInfoText.StringReference = new LocalizedString("PieceInfo", $"{playerPiece.Kind}");
        Game.EnemyChoiceNameText.StringReference = new LocalizedString("PieceNames", $"{enemyPiece.Kind}");
        Game.EnemyChoiceInfoText.StringReference = new LocalizedString("PieceInfo", $"{enemyPiece.Kind}");
    }

    public override void ApplyChoice(int choice)
    {
        // Get the pieces corresponding to the selected option
        AssetGroup.Piece playerPiece;
        AssetGroup.Piece enemyPiece;
        playerPiece = PlayerPieces[Game.ChoiceButtons.SelectedIndex];
        enemyPiece = EnemyPieces[Game.ChoiceButtons.SelectedIndex];

        // Log the selected pieces
        Debug.Log($"Enemy Upgrade: {enemyPiece}");
        Debug.Log($"Player Upgrade: {playerPiece}");

        // Add the enemy piece, removing a lower value piece if necessary
        if (Game.EnemyPieces.Count == 16) RemoveLowerValue(enemyPiece);
        if (Game.EnemyPieces.Count < 16) Game.EnemyPieces.Add(enemyPiece);

        // Add the player piece as long a sideboard space is empty
        if (Game.PlayerSideBoard.Count < 24) Game.PlayerSideBoard.Add(new PiecePositionRecord(playerPiece, null));
    }

    /// <summary>Create a piece sprite</summary>
    /// <param name="choice">The choice to create the sprite for</param>
    /// <param name="player">True if this is the player's half of the choice</param>
    /// <param name="info">True if this is going to be shown in the info menu</param>
    /// <returns>A new Piece</returns>
    private Piece CreatePiece(int choice, bool player, bool info)
    {
        Piece piece;
        if (player)
        {
            piece = Game.CreatePiece(PlayerPieces[choice], true, GameState.IsPlayerWhite);
        }
        else
        {
            piece = Game.CreatePiece(EnemyPieces[choice], false, !GameState.IsPlayerWhite);
        }
        piece.IsPlayer = player;
        if (info)
        {
            piece.WarpTo(InfoPosition(player, false, false));
            InfoSprites.Add(piece.gameObject);
        }
        else
        {
            piece.WarpTo(ChoicePosition(choice, player, false));
            Sprites.Add(piece.gameObject);
        }
        return piece;
    }

    /// <summary>Remove an enemy piece with lower value than the given kind</summary>
    /// <param name="kind">The kind of piece to make space for</param>
    public void RemoveLowerValue(AssetGroup.Piece kind)
    {
        int valueToAdd = Game.CurrentSet.GetPieceValue(kind);
        for (int i = 0; i < Game.EnemyPieces.Count; i++)
        {
            int valueToRemove = Game.CurrentSet.GetPieceValue(Game.EnemyPieces[i]);
            if (valueToRemove < valueToAdd)
            {
                Game.EnemyPieces.RemoveAt(i);
                return;
            }
        }
    }
}
