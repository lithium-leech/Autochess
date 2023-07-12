using UnityEngine;

/// <summary>
/// An object which takes up space on the chess board, but is not controlled by either player
/// </summary>
public abstract class Terrain : ChessObject
{
    /// <summary>The kind of terrain this is</summary>
    public abstract AssetGroup.Object Kind { get; }

    /// <summary>True if this terrain can be passed through</summary>
    public abstract bool Passable { get; }

    protected override void Update2() { }

    public override void Destroy()
    {
        if (Space != null) 
            Space.RemoveObject(this);
        GameObject.Destroy(gameObject);
    }
}
