/// <summary>
/// A power which gives an additional row for placing pieces
/// </summary>
public class ExtraRow : Power
{
    public override AssetGroup.Power Kind { get { return AssetGroup.Power.ExtraRow; } }

    public override void Activate()
    {
        // The power doesn't work if there are no more available rows
        if (Game.GameBoard.PlayerRows + Game.GameBoard.EnemyRows >= Game.GameBoard.Height) return;

        // Add a an extra row for the player with the power
        if (IsPlayer)
        {
            Game.GameBoard.PlayerRows += 1;
        }
        else
        {
            Game.GameBoard.EnemyRows += 1;
        }
    }
}
