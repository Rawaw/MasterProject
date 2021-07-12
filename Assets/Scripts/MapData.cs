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
    int maxCoins;
    int collectedCoins;
    int bestTime;

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
    public void SetMaxCoins(int coins){
        maxCoins = coins;
    }
    public void SetCollectedCoins(int coins){
        collectedCoins = coins;
    }
    public void SetMapTime(int time){
        bestTime = time;
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
    public int GetCollectedCoins(){
        return collectedCoins;
    }
    public int GetMaxCoins(){
        return maxCoins;
    }
    public int GetBestTime(){
        return bestTime;
    }

}
