using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// A class containing Editor scripts that aid in the process of creating new chess objects
/// </summary>
public class PrefabCreator : EditorWindow
{
    /// <summary>Creates a new prefab for the selected sprites</summary>
    [MenuItem("Assets/Create New Prefab")]
    static void CreatePrefabs()
    {
        // Make sure all of the selected objects are of type Sprite
        foreach (UnityEngine.Object selectedObject in Selection.objects)
            if (selectedObject is not Sprite) throw new Exception("All selected objects must be of type Sprite");

        // Open a custom Editor Window to prompt the user
        PrefabCreator window = GetWindow<PrefabCreator>();
        window.Show();
    }

    /// <summary>A target asset group to create new prefabs for</summary>
    private AssetGroup.Group Group = AssetGroup.Group.None;

    /// <summary>Runs when the editor window is shown</summary>
    private void OnGUI()
    {
        // Ask the user for the group that these prefabs will belong to
        Group = (AssetGroup.Group)EditorGUILayout.EnumPopup("Select the Group to create these assets for:", Group);

        // Check if the user clicks the "Create" button
        if (GUILayout.Button("Create"))
        {
            // Iterate over each selected Sprite
            foreach (Sprite sprite in Selection.objects.Cast<Sprite>())
            {
                // Establish names and paths
                FileInfo file = new FileInfo(AssetDatabase.GetAssetPath(sprite));
                string spriteName = Path.GetFileNameWithoutExtension(file.Name);
                string objectName = spriteName.Replace("White","").Replace("Black","").Replace("Green","").Replace("Red","");
                string prefabGroup = Group.ToString().Replace("White","").Replace("Black","").Replace("Green","").Replace("Red","") + "s";
                string path = Path.Combine(Application.dataPath, "Prefab", prefabGroup, spriteName + ".prefab");
                if (new FileInfo(path).Exists) throw new Exception($"{spriteName} already has a prefab in {prefabGroup}");

                // Check for a matching identity in the given group
                string[] identities = Group switch {
                    AssetGroup.Group.Tile => Enum.GetNames(typeof(AssetGroup.Tile)),
                    AssetGroup.Group.Panel => Enum.GetNames(typeof(AssetGroup.Panel)),
                    AssetGroup.Group.GreenHighlight => Enum.GetNames(typeof(AssetGroup.Highlight)),
                    AssetGroup.Group.RedHighlight => Enum.GetNames(typeof(AssetGroup.Highlight)),
                    AssetGroup.Group.WhitePiece => Enum.GetNames(typeof(AssetGroup.Piece)),
                    AssetGroup.Group.BlackPiece => Enum.GetNames(typeof(AssetGroup.Piece)),
                    AssetGroup.Group.WhiteObject => Enum.GetNames(typeof(AssetGroup.Object)),
                    AssetGroup.Group.BlackObject => Enum.GetNames(typeof(AssetGroup.Object)),
                    AssetGroup.Group.Power => Enum.GetNames(typeof(AssetGroup.Power)),
                    AssetGroup.Group.Map => Enum.GetNames(typeof(AssetGroup.Map)),
                    AssetGroup.Group.Set => Enum.GetNames(typeof(AssetGroup.Set)),
                    _ => throw new Exception($"{Group} not recognized")
                };
                int identity = 0;
                for (int i = 0; i < identities.Length; i++)
                if (identities[i] == objectName)
                {
                    identity = i;
                    break;
                }
                if (identity == 0) throw new Exception($"{objectName} not found in {Group}");

                // Create a prefab template
                GameObject template = new GameObject(objectName);
                template.transform.position = new Vector3(100, 100, 100);

                // Attach the sprite
                SpriteRenderer renderer = template.AddComponent<SpriteRenderer>();
                renderer.sprite = sprite;

                // Add the asset keys
                AssetKey key = Group switch
                {
                    AssetGroup.Group.Tile => template.AddComponent<TileKey>(),
                    AssetGroup.Group.Panel => template.AddComponent<PanelKey>(),
                    AssetGroup.Group.GreenHighlight => template.AddComponent<HighlightKey>(),
                    AssetGroup.Group.RedHighlight => template.AddComponent<HighlightKey>(),
                    AssetGroup.Group.WhitePiece => template.AddComponent<PieceKey>(),
                    AssetGroup.Group.BlackPiece => template.AddComponent<PieceKey>(),
                    AssetGroup.Group.WhiteObject => template.AddComponent<ObjectKey>(),
                    AssetGroup.Group.BlackObject => template.AddComponent<ObjectKey>(),
                    AssetGroup.Group.Power => template.AddComponent<PowerKey>(),
                    AssetGroup.Group.Map => template.AddComponent<MapKey>(),
                    AssetGroup.Group.Set => template.AddComponent<SetKey>(),
                    _ => throw new Exception($"{Group} not recognized")
                };
                key.Group = Group;
                key.ID = identity;

                // Create the new prefab
                PrefabUtility.SaveAsPrefabAsset(template, path);
                DestroyImmediate(template);
                AssetDatabase.Refresh();
            }

            // Close the custom Editor Window
            Close();
        }
    }
}
