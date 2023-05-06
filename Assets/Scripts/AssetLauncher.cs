using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class AssetLauncher : MonoBehaviour
{
    /// Properties to set using Unity interface
    public AssetLabelReference LabelReference;
    public AssetGroup.Scene Kind;
    public GameObject LoadingBar;

    /// <summary>The loading bar's slider</summary>
    private Slider Slider { get; set; }
    
    void Start()
    {
        // Load assets if necessary
        if (!AssetManager.LoadOperation.IsValid())
        {
            Debug.Log("Loading assets...");
            AssetManager.LoadAssetsAsync(LabelReference);
        }
        Slider = LoadingBar.GetComponent<Slider>();
    }

    void Update()
    {
        // Wait for assets to finish loading
        if (AssetManager.LoadOperation.IsDone)
        {
            // Instantiate the targeted asset
            try
            {
                if (Kind == AssetGroup.Scene.None)
                {
                    Debug.LogWarning("No scene asset has been selected for launch.");
                }
                else
                {
                    Debug.Log($"Launching scene {Kind}");
                    GameObject asset = AssetManager.Prefabs[AssetGroup.Group.Scene][(int)Kind];
                    Instantiate(asset);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to launch scene {Kind}: {e.Message}");
                throw e;
            }
            finally
            {
                Destroy(LoadingBar);
                Destroy(this);
            }
        }
        else
        {
            // Update the loading bar
            Slider.value = AssetManager.LoadOperation.PercentComplete;
        }
    }
}
