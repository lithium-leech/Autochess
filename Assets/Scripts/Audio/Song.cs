using System;
using UnityEngine;

/// <summary>
/// A playable song
/// </summary>
[Serializable]
public class Song
{
    /// Properties to set using Unity interface
    public SongName Name;
    public AudioSource Source;
}
