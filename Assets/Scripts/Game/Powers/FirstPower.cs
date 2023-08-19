/// <summary>
/// A power which steals the first turn
/// </summary>
public class FirstPower : Power
{
    public override AssetGroup.Power Kind { get { return AssetGroup.Power.First; } }

    public override void Activate()
    {
        // Switch player colors
        GameState.IsPlayerWhite = !GameState.IsPlayerWhite;
        
        // Base activation
        base.Activate();
    }

    public override void Deactivate()
    {
        // Switch player colors
        GameState.IsPlayerWhite = !GameState.IsPlayerWhite;
        
        // Base deactivation
        base.Deactivate();
    }
}
