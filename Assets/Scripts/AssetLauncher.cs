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
                    Debug.Log("No scene assets have been selected for launch");
                }
                else
                {
                    GameObject asset = AssetManager.Prefabs[AssetGroup.Groups.Scene][(int)Kind];
                    Instantiate(asset);
                }
            }
            catch (Exception e)
            {
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
