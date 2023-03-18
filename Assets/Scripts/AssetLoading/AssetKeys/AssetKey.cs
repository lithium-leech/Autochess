using UnityEngine;

/// <summary>
/// A key to attach to assets in order to load them properly
/// </summary>
public abstract class AssetKey : MonoBehaviour
{
    /// Properties to set using Unity interface
    public AssetGroup.Groups Group;

    // The secondary key used to identify an asset within its group
    public abstract int ID { get; }
}
