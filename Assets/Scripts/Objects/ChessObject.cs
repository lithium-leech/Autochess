using UnityEngine;

/// <summary>
/// A single game object
/// </summary>
public abstract class ChessObject : MonoBehaviour
{
    /// <summary>Initializes the ChessObject</summary>
    /// <param name="game">The game this object is a part of</param>
    /// <param name="player">True if the object is controlled by the player</param>
    /// <param name="white">True if the object is white</param>
    /// <remarks>Should be called as close to creation as possible</remarks>
    public virtual void Initialize(Game game, bool player, bool white)
    {
        Game = game;
        IsPlayer = player;
        IsWhite = white;
        IsGrabable = player;
    }

    /// <summary>The game this object is a part of</summary>
    public Game Game { get; set; }

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

    public virtual void Update() { }

    /// <summary>Determines if this object can be placed in a given space</summary>
    /// <param name="space">The space to check</param>
    /// <returns>True if this object can be placed in the given space</returns>
    public virtual bool IsPlaceable(Space space) => space.InPlayerZone();

    /// <summary>Removes this object from the entire game</summary>
    public virtual void Destroy()
    {
        Space?.RemoveObject(this);
        GameObject.Destroy(gameObject);
    }

    /// <summary>Warps the object to the specified location</summary>
    /// <param name="space">The world coordinates to warp to</param>
    public void WarpTo(Vector3 position) => transform.position = position;
}
