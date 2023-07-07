using UnityEngine;

/// <summary>
/// An object which takes up space on the chess board, but is not controlled by either player
/// </summary>
public abstract class Terrain : ChessObject
{
    /// <summary>The kind of terrain this is</summary>
    public abstract AssetGroup.Object Kind { get; }

    protected override void Update2() { }

    public override void Remove()
    {
        if (Board != null)
        {
            Board.Spaces[Space.x, Space.y].Remove(this);
            Board = null;
            Space = new Vector2Int(-1, -1);
        }
    }
}
