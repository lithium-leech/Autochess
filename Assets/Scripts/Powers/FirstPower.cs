/// <summary>
/// A power which steals the first turn
/// </summary>
public class FirstPower : Power
{
    public override AssetGroup.Power Kind { get { return AssetGroup.Power.First; } }

    public override AssetGroup.Power RemoveKind { get { throw new System.Exception("The First power has no remove kind."); } }

    public override void Activate()
    {
        // Switch player colors
        if (IsPlayer) GameState.IsPlayerWhite = true;
        else GameState.IsPlayerWhite = false;

        // Destroy this power
        Game.Destroy(this.gameObject);
    }

    public override void Deactivate()
    {
        throw new System.Exception("The First power cannot be deactivated.");
    }
}
