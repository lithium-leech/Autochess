using UnityEngine;

/// <summary>An object for recording the position of a piece</summary>
public class PositionRecord
{
    /// <summary>Creates a new instance of a PositionRecord</summary>
    /// <param name="kind">The kind of piece</param>
    /// <param name="space">The piece's position</param>
    public PositionRecord(AssetGroup.Piece kind, Vector2Int? space)
    {
        Kind = kind;
        Space = space;
    }

    /// <summary>The kind of piece</summary>
    public AssetGroup.Piece Kind;

    /// <summary>The piece's position</summary>
    public Vector2Int? Space;
}
