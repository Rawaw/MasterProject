using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{

    public struct sign{
        public int x;
        public int y;
        public string message;
    }

    Vector2Int mapSize;
    int signAmount;
    int[,] foreMap;
    int[,] backMap;
    sign[] signs;
    
    public void ResetData(){
        mapSize = Vector2Int.zero;
        signAmount = 0;
        foreMap = new int[,]{};
        backMap = new int[,]{};
        signs = new sign[]{};
    }

    public void SetMapSize(Vector2Int size){
        mapSize = size;
        foreMap = new int[mapSize.x,mapSize.y];
        backMap = new int[mapSize.x,mapSize.y];
    }
    public void SetForeMap(int[,] map){
        foreMap = map;
    }
    public void SetBackMap(int[,] map){
        backMap = map;
    }
    public void SetSignAmount(int amount){
        signAmount = amount;
        signs = new sign[signAmount];
    }
    public void SetSigns(sign[] signList){
        signs = signList;
    }

    public Vector2Int GetMapSize(){
        return mapSize;
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
    public sign[] GetSignsList(){
        return signs;
    }

}
