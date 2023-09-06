/// <summary>
/// A pair of pieces, one for the player and one for the opponent
/// </summary>
public class PiecePair
{
    /// <summary>Creates a new instance of a piece pair</summary>
    /// <param name="player">The player piece</param>
    /// <param name="enemy">The enemy piece</param>
    public PiecePair(AssetGroup.Piece player, AssetGroup.Piece enemy)
    {
        Player = player;
        Enemy = enemy;
    }

    /// <summary>The piece designated for the player</summary>
    public AssetGroup.Piece Player { get; }

    /// <summary>The piece designated for the enemy</summary>
    public AssetGroup.Piece Enemy { get; }
}
