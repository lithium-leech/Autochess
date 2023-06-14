/// <summary>
/// A power which steals the first turn
/// </summary>
public class First : Power
{
    public override AssetGroup.Power Kind { get { return AssetGroup.Power.First; } }

    public override void Activate()
    {
        if (IsPlayer) GameState.IsPlayerWhite = true;
        else GameState.IsPlayerWhite = false;
    }
}
