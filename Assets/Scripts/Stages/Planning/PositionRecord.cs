using System;
using UnityEngine;

/// <summary>An object for recording the position of a piece</summary>
public class PositionRecord
{
    /// <summary>Creates a new instance of a PositionRecord</summary>
    /// <param name="type">The Type of peice</param>
    /// <param name="space">The peice's position</param>
    public PositionRecord(Type type, Vector2Int? space)
    {
        Type= type;
        Space= space;
    }

    /// <summary>The Type of peice</summary>
    public Type Type;

    /// <summary>The peice's position</summary>
    public Vector2Int? Space;
}
