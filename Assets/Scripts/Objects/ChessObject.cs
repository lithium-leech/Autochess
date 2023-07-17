using UnityEngine;

/// <summary>
/// A single game object
/// </summary>
public abstract class ChessObject : MonoBehaviour
{
    /// <summary>The board this object is on</summary>
    public Board Board { get; set; }

    /// <summary>The object's space on the board</summary>
    public Space Space { get; set; }

/// <summary>True if the object is controlled by the player</summary>
    public bool IsPlayer { get; set; }
    
    /// <summary>True if the object is white</summary>
    public bool IsWhite { get; set; }

    /// <summary>True if this object can be picked up by the player</summary>
    public bool IsGrabable { get; set; }

    public abstract void Update();

    /// <summary>Removes this object from the entire game</summary>
    public abstract void Destroy();

    /// <summary>Warps the object to the specified location</summary>
    /// <param name="space">The world coordinates to warp to</param>
    public void WarpTo(Vector3 position) => transform.position = position;
}
