using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Tilemaps;

public class SpecialTile : Tile
{
    
    public string description = "Default";

    public string getDescription(){
        return description;
    }

    #if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a RoadTile Asset
    [MenuItem("Assets/Create/SpecialTile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Special Tile", "New Special Tile", "Asset", "Save Special Tile", "Assets");
        if (path == "")
            return;
    AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<SpecialTile>(), path);
    }
#endif

}
