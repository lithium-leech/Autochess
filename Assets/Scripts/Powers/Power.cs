using UnityEngine;

/// <summary>
/// A single game power
/// </summary>
public abstract class Power : MonoBehaviour
{
    /// <summary>The game this power is being used in</summary>
    public Game Game { get; set; }

    /// <summary>True if this power is for the player</summary>
    public bool IsPlayer { get; set; }

    /// <summary>The kind of power this is</summary>
    public abstract AssetGroup.Power Kind { get; }

    /// <summary>Applies the power to the current game</summary>
    public abstract void Activate();

    /// <summary>Unapplies the power from the current game</summary>
    // public abstract void Deactivate();

    /// <summary>Warps the piece to the specified location</summary>
    /// <param name="space">The world coordinates to warp to</param>
    public void WarpTo(Vector3 position)
    {
        transform.position = position;
    }
}
