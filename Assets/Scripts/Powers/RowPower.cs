using UnityEngine;

/// <summary>
/// A power which gives an additional row for placing pieces
/// </summary>
public class RowPower : Power
{
    public override AssetGroup.Power Kind { get { return AssetGroup.Power.Row; } }
    
    public override void Activate()
    {
        // The power doesn't work if there are no more available rows
        if (Game.GameBoard.PlayerRows + Game.GameBoard.EnemyRows >= Game.GameBoard.Height) return;

        // Add a row to the player with the power
        if (IsPlayer) Game.GameBoard.PlayerRows += 1;
        else Game.GameBoard.EnemyRows += 1;

        // Add this power to the accumulated powers list
        if (IsPlayer) Game.PlayerPowers.Add(this);
        else Game.EnemyPowers.Add(this);
        WarpTo(GameState.ShadowZone);
    }
    
    public override void Deactivate()
    {
        // The power doesn't work if the rows would be reduced below 2
        if (IsPlayer && Game.GameBoard.PlayerRows < 3) return;
        if (!IsPlayer && Game.GameBoard.EnemyRows < 3) return;

        // Remove a row from the player with the power
        if (IsPlayer) Game.GameBoard.PlayerRows -= 1;
        else Game.GameBoard.EnemyRows -= 1;

        // Remove this power from the accumulated powers list
        if (IsPlayer) Game.PlayerPowers.Remove(this);
        else Game.EnemyPowers.Remove(this);

        // Destroy this power
        Game.Destroy(this.gameObject);
    }
}
