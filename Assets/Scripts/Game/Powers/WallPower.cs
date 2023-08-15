using UnityEngine;

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

        // Add this power to the accumulated powers list
        if (IsPlayer) Game.PlayerPowers.Add(this);
        else Game.EnemyPowers.Add(this);
        WarpTo(GameState.ShadowZone);
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

        // Remove this power from the accumulated powers list
        if (IsPlayer) Game.PlayerPowers.Remove(this);
        else Game.EnemyPowers.Remove(this);

        // Destroy this power
        Game.Destroy(this.gameObject);
    }
}
