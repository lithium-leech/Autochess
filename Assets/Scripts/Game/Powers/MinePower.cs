/// <summary>
/// A power which allows a land mine to be placed on the map
/// </summary>
public class MinePower : Power
{
    public override AssetGroup.Power Kind { get { return AssetGroup.Power.Mine; } }

    public override void Activate()
    {
        // Create a mine
        if (IsPlayer) Game.PlayerObjects.Add(AssetGroup.Object.Mine);
        else Game.EnemyObjects.Add(AssetGroup.Object.Mine);
        
        // Base activation
        base.Activate();
    }
    
    public override void Deactivate()
    {
        // Take away a mine
        if (IsPlayer) Game.PlayerObjects.Remove(AssetGroup.Object.Mine);
        else Game.EnemyObjects.Remove(AssetGroup.Object.Mine);
        
        // Base deactivation
        base.Deactivate();
    }
}
