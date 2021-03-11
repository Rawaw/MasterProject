using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapUtils
{
    
    public static void BoxFill(Tilemap map, TileBase tile, Vector3Int startPos, Vector3Int endPos){
        int rows = endPos.x - startPos.x;
        int columns = endPos.y - startPos.y;
        for(int i = 0; i <= rows;i++){
            for(int j = 0; j <= columns;j++){
                Vector3Int fillPos = new Vector3Int(startPos.x + i, startPos.y + j, startPos.z);
                map.SetTile(fillPos,tile);
            }
        }
    }

}
