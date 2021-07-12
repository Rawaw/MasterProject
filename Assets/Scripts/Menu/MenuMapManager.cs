using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MenuMapManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!Directory.Exists(Application.dataPath + "/CustomMaps/"))
        {    
            Directory.CreateDirectory(Application.dataPath + "/CustomMaps/");
            Debug.Log("Created directory: " + Application.dataPath + "/CustomMaps/");
        }
        if(!Directory.Exists(Application.dataPath + "/Maps/"))
        {    
            Directory.CreateDirectory(Application.dataPath + "/Maps/");
            Debug.Log("Created directory: " + Application.dataPath + "/Maps/");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<string> GetMapList(string folder){
        return MapFileManager.LoadMapList(folder);
    }

    public int[] GetCoinInfo(string name, string folder){
        MapData mapData = MapFileManager.LoadMap(folder, name);
        int[] coins = new int[2];
        coins[0] = mapData.GetCollectedCoins();
        coins[1] = mapData.GetMaxCoins();
        return coins;
    }

    public int GetTimeInfo(string name, string folder){
        MapData mapData = MapFileManager.LoadMap(folder, name);
        int time = mapData.GetBestTime();
        return time;
    }

    public MapUtils.mapInfo LoadMapInfo(string name, string folder){
        MapData map = MapFileManager.LoadMap(folder, name);
        MapUtils.mapInfo info = new MapUtils.mapInfo();
        return info;
    }
}
