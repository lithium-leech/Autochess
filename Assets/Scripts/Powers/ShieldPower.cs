using UnityEngine;

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

        // Add this power to the accumulated powers list
        if (IsPlayer) Game.PlayerPowers.Add(this);
        else Game.EnemyPowers.Add(this);
        WarpTo(GameState.ShadowZone);
    }
    
    public override void Deactivate()
    {
        // Take away a shield
        if (IsPlayer) Game.PlayerObjects.Remove(AssetGroup.Object.Shield);
        else Game.EnemyObjects.Remove(AssetGroup.Object.Shield);

        // Remove this power from the accumulated powers list
        if (IsPlayer) Game.PlayerPowers.Remove(this);
        else Game.EnemyPowers.Remove(this);

        // Destroy this power
        Game.Destroy(this.gameObject);
    }
}
