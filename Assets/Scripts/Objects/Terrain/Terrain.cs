using UnityEngine;

/// <summary>
/// An object which takes up space on the chess board, but is not controlled by either player
/// </summary>
public abstract class Terrain : ChessObject
{
    /// <summary>The kind of terrain this is</summary>
    public abstract AssetGroup.Object Kind { get; }

    public override bool IsPlaceable(Space space) => space.InPlayerZone() || space.InNeutralZone();

    /// <summary>True if this terrain can be entered</summary>
    /// <param name="piece">The object that wants to enter</param>
    public virtual bool IsEnterable(ChessObject obj) => true;

    /// <summary>Performs the effects of this terrain when a piece enters</summary>
    /// <param name="piece">The piece entering</param>
    public virtual void OnEnter(Piece piece) { }

    /// <summary>Performs the effects of this terrain when a piece exits</summary>
    /// /// <param name="piece">The piece exiting</param>
    public virtual void OnExit(Piece piece) { }
}
