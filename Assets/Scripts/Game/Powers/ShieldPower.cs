/// <summary>
/// A power which protects a selected piece at the start of each battle
/// </summary>
public class ShieldPower : Power
{
    public override AssetGroup.Power Kind { get { return AssetGroup.Power.Shield; } }

    public override void Activate()
    {
        // Create a shield
        if (IsPlayer) Game.PlayerObjects.Add(AssetGroup.Object.Shield);
        else Game.EnemyObjects.Add(AssetGroup.Object.Shield);

        // Base activation
        base.Activate();
    }
    
    public override void Deactivate()
    {
        // Take away a shield
        if (IsPlayer) Game.PlayerObjects.Remove(AssetGroup.Object.Shield);
        else Game.EnemyObjects.Remove(AssetGroup.Object.Shield);

        // Base deactivation
        base.Deactivate();
    }
}
