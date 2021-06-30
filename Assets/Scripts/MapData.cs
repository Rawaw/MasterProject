using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
    string mapName;

    int mapSizeX;
    int mapSizeY;
    int signAmount;
    int[,] foreMap;
    int[,] backMap;
    MapUtils.sign[] signs;
    
    public void ResetData(){
        mapSizeX = 0;
        mapSizeY = 0;
        signAmount = 0;
        foreMap = new int[,]{};
        backMap = new int[,]{};
        signs = new MapUtils.sign[]{};
    }

    public void SetMapName(string name){
        mapName = name;
    }
    public void SetMapSize(Vector2Int size){
        mapSizeX = size.x;
        mapSizeY = size.y;
        //foreMap = new int[mapSizeX,mapSizeY];
        //backMap = new int[mapSizeX,mapSizeY];
    }
    public void SetForeMap(int[,] map){
        foreMap = map;
    }
    public void SetBackMap(int[,] map){
        backMap = map;
    }
    public void SetSignAmount(int amount){
        signAmount = amount;
        signs = new MapUtils.sign[signAmount];
    }
    public void SetSigns(MapUtils.sign[] signList){
        signs = signList;
    }

    public string GetMapName(){
        return mapName;
    }
    public Vector2Int GetMapSize(){
        Vector2Int size = new Vector2Int(mapSizeX,mapSizeY);
        return size;
    }
    public int GetSignAmount(){
        return signAmount;
    }
    public int[,] GetForeMap(){
        return foreMap;
    }
    public int[,] GetBackMap(){
        return backMap;
    }
    public MapUtils.sign[] GetSignsList(){
        return signs;
    }

}
