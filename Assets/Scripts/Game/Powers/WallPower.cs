/// <summary>
/// A power which gives the player walls to be placed on the map
/// </summary>
public class WallPower : Power
{
    public override AssetGroup.Power Kind { get { return AssetGroup.Power.Wall; } }

    public override void Activate()
    {
        // Create two new walls
        if (IsPlayer)
        {
            Game.PlayerObjects.Add(AssetGroup.Object.Wall);
            Game.PlayerObjects.Add(AssetGroup.Object.Wall);
        }
        else
        {
            Game.EnemyObjects.Add(AssetGroup.Object.Wall);
            Game.EnemyObjects.Add(AssetGroup.Object.Wall);
        }

        // Base activation
        base.Activate();
    }
    
    public override void Deactivate()
    {
        // Take away two walls
        if (IsPlayer)
        {
            Game.PlayerObjects.Remove(AssetGroup.Object.Wall);
            Game.PlayerObjects.Remove(AssetGroup.Object.Wall);
        }
        else
        {
            Game.EnemyObjects.Remove(AssetGroup.Object.Wall);
            Game.EnemyObjects.Remove(AssetGroup.Object.Wall);
        }

        // Base deactivation
        base.Deactivate();
    }
}
