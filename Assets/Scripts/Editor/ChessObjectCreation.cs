using System.Linq;
using UnityEditor;
using UnityEngine;

public class ChessObjectCreation
{
    [MenuItem("Assets/Set Default Values")]
    static void SetDefault()
    {
        // Make sure all of the selected objects are Texture2D
        foreach (Object selectedObject in Selection.objects)
            if (selectedObject is not Texture2D) throw new System.Exception("All selected objects must of the type Texture2D");

        // Set the default values for each selected object
        foreach (Texture2D texture in Selection.objects.Cast<Texture2D>())
        {
            string assetPath = AssetDatabase.GetAssetPath(texture);
            TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (textureImporter != null)
            {
                textureImporter.spritePixelsPerUnit = 32;
                textureImporter.filterMode = FilterMode.Point;
                textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
                AssetDatabase.ImportAsset(assetPath);
            }
        }
    }

    [MenuItem("Assets/Create New Prefab")]
    static void CreatePrefab()
    {
        // Make sure the selected object is a Texture2D
        Texture2D selectedObject = Selection.activeObject as Texture2D;
        if (selectedObject == null) throw new System.Exception("The selected Object must be a Texture2D");
        // Create a Prefab from the selected GameObject
        // GameObject prefab = PrefabUtility.SaveAsPrefabAsset(selectedObject, "Assets/Prefabs/NewPrefab.prefab");
        // Debug.Log("Prefab created: " + AssetDatabase.GetAssetPath(prefab));
        Debug.Log($"Prefab created: {AssetDatabase.GetAssetPath(selectedObject)}");
    }
}
