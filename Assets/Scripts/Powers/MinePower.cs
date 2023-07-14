using UnityEngine;

/// <summary>
/// A power which allows a land mine to be placed on the map
/// </summary>
public class MinePower : Power
{
    public override AssetGroup.Power Kind { get { return AssetGroup.Power.Mine; } }

    public override AssetGroup.Power RemoveKind { get { return AssetGroup.Power.RemoveMine; } }

    public override void Activate()
    {
        // Create two new walls
        if (IsPlayer) Game.PlayerObjects.Add(AssetGroup.Object.Mine);
        else Game.EnemyObjects.Add(AssetGroup.Object.Mine);

        // Add this power to the accumulated powers list
        if (IsPlayer) Game.PlayerPowers.Add(this);
        else Game.EnemyPowers.Add(this);
        WarpTo(new Vector3(100, 100, 100));
    }
    
    public override void Deactivate()
    {
        // Take away two walls
        if (IsPlayer) Game.PlayerObjects.Remove(AssetGroup.Object.Mine);
        else Game.EnemyObjects.Remove(AssetGroup.Object.Mine);

        // Remove this power from the accumulated powers list
        if (IsPlayer) Game.PlayerPowers.Remove(this);
        else Game.EnemyPowers.Remove(this);

        // Destroy this power
        Game.Destroy(this.gameObject);
    }
}
