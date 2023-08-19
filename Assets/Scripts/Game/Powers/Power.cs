/// <summary>
/// A single game power
/// </summary>
public abstract class Power : ChessObject
{
    /// <summary>The group that this power belongs to (normal or remove)</summary>
    public AssetGroup.Group Group { get; set; }

    /// <summary>The kind of power this is</summary>
    public abstract AssetGroup.Power Kind { get; }

    public override void Initialize(Game game, bool player, bool white)
    {
        Game = game;
        IsWhite = white;
        IsPlayer = player;
        IsGrabable = false;
    }

    /// <summary>Applies the power to the current game</summary>
    public virtual void Activate()
    {
        // Add this power to the game
        if (IsPlayer)
        {
            Game.PlayerPowers.Add(this);
            Game.PlayerPowerBoard.AddPower(this);
        }
        else
        {
            Game.EnemyPowers.Add(this);
            Game.EnemyPowerBoard.AddPower(this);
        }
    }

    /// <summary>Unapplies this power from the current game</summary>
    public virtual void Deactivate()
    {
        // Remove this power from the game
        if (IsPlayer)
        {
            Game.PlayerPowers.Remove(this);
        }
        else
        {
            Game.EnemyPowers.Remove(this);
        }
        Space.RemoveObject(this);

        // Destroy this power
        Game.Destroy(this.gameObject);
    }
}
