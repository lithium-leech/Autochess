using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>Class for loading and instantiating assets</summary>
public static class AssetManager
{
    /// <summary>A collection with all of the loaded prefabs</summary>
    public static IDictionary<AssetGroup.Groups, IDictionary<int, GameObject>> Prefabs { get; } = new Dictionary<AssetGroup.Groups, IDictionary<int, GameObject>>();

    /// <summary>The operation that is loading assets</summary>
    public static AsyncOperationHandle<IList<GameObject>> LoadOperation { get; set; }

    /// <summary>Starts loading assets</summary>
    /// <param name="references">A collection of asset references</param>
    /// <returns>The loading operation</returns>
    public static void LoadAssetsAsync(AssetLabelReference labelReference)
    {
        // Initialize dictionary groups
        foreach (AssetGroup.Groups type in Enum.GetValues(typeof(AssetGroup.Groups)))
            if (type != AssetGroup.Groups.None)
                if (!Prefabs.ContainsKey(type))
                    Prefabs.Add(type, new Dictionary<int, GameObject>());

        // Load assets into dictionary
        LoadOperation = Addressables.LoadAssetsAsync<GameObject>(labelReference, (asset) =>
        {
            if (asset.TryGetComponent<AssetKey>(out var key))
            {
                if (key.Group == AssetGroup.Groups.None)
                    Debug.Log($"Unable to find the group for: {asset.name}");
                else if (key.ID == 0)
                    Debug.Log($"Unable to find the identifier for: {asset.name}");
                else
                    Prefabs[key.Group].Add(key.ID, asset);
            }
            else
            {
                Debug.Log($"Unable to find a key for: {asset.name}");
            }
        });
    }
}
