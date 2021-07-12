using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapUtils
{
    [System.Serializable]
    public struct sign{
        public int x;
        public int y;
        public string message;
    }

    public struct mapInfo{
        public string name;
        //public int time;
    }

    static public int GetTileId(TileBase tile){
        if(tile != null){
            switch(tile.name){
                case "GrassLandTile":
                    return 1;
                case "ForestLandTile":
                    return 2;
                case "RockLandTile":
                    return 3;
                case "Spikes":
                    return 11;
                case "Water16x16":
                    return 12;
                case "Water":
                    return 12;
                case "Boots":
                    return 21;
                case "Glove":
                    return 22;
                case "Feather":
                    return 23;
                case "Orb":
                    return 24;
                case "Coin":
                    return 25;
                case "Player":
                    return 31;
                case "FinalDoor":
                    return 32;
                case "Sign":
                    return 33;
                case "CheckPoint":
                    return 34;
                default:
                    return 99;
            }
        }else{
            return 0;
        }
    }
}
