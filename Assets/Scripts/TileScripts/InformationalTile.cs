using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Tilemaps;

public class InformationalTile : Tile
{
    
    //public Sprite m_Sprite;

    public int getSomeIntel(){
        return 123;
    }

    #if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a RoadTile Asset
    [MenuItem("Assets/Create/InfoTile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Info Tile", "New Info Tile", "Asset", "Save Info Tile", "Assets");
        if (path == "")
            return;
    AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<InformationalTile>(), path);
    }
#endif

}
