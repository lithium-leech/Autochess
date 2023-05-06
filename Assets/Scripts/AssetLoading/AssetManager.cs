using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>Class for loading and instantiating assets</summary>
public static class AssetManager
{
    /// <summary>A collection with all of the loaded prefabs</summary>
    public static IDictionary<AssetGroup.Group, IDictionary<int, GameObject>> Prefabs { get; } = new Dictionary<AssetGroup.Group, IDictionary<int, GameObject>>();

    /// <summary>The operation that is loading assets</summary>
    public static AsyncOperationHandle<IList<GameObject>> LoadOperation { get; set; }

    /// <summary>Starts loading assets</summary>
    /// <param name="references">A collection of asset references</param>
    /// <returns>The loading operation</returns>
    public static void LoadAssetsAsync(AssetLabelReference labelReference)
    {
        // Initialize dictionary groups
        foreach (AssetGroup.Group type in Enum.GetValues(typeof(AssetGroup.Group)))
            if (type != AssetGroup.Group.None)
                if (!Prefabs.ContainsKey(type))
                    Prefabs.Add(type, new Dictionary<int, GameObject>());

        // Load assets into dictionary
        LoadOperation = Addressables.LoadAssetsAsync<GameObject>(labelReference, (asset) =>
        {
            if (asset.TryGetComponent<AssetKey>(out var key))
            {
                if (key.Group == AssetGroup.Group.None)
                    Debug.LogWarning($"Unable to find the asset group for: {asset.name}");
                else if (key.ID == 0)
                    Debug.LogWarning($"Unable to find the asset identifier for: {asset.name}");
                else
                    Prefabs[key.Group].Add(key.ID, asset);
            }
            else
            {
                Debug.LogWarning($"Unable to find an asset key for: {asset.name}");
            }
        });
    }
}
