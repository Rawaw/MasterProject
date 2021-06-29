using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MapFileManager : MonoBehaviour
{
    static string mapName = "NoName";

    static string customMapPath = Application.persistentDataPath + "/CustomMaps/";
    string saveLocation = customMapPath + mapName + ".dat";

    MapData mapData;
    List<string> mapNames;

    void Start()
    {
        mapNames = new List<string>();
    }

    public void SaveMap(){
        Debug.Log("Saving map as: " + mapName);
        string saveLocation = Application.persistentDataPath + "/CustomMaps/" + mapName + ".dat";
        FileStream file;
 
        if(File.Exists(saveLocation)) 
            file = File.OpenWrite(saveLocation);
        else 
            file = File.Create(saveLocation);
 
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, mapData);
        file.Close();
        Debug.Log("Map saved");
    }

    public void SetMapName(string name){
        mapName = name;
    }

    public void SetMapData(MapData map){
        mapData = map;
    }

    void LoadMapList(string path){
        mapNames.Clear();
        foreach (string file in System.IO.Directory.GetFiles(path)){
            Debug.Log("checking file: " + file);
            if(file.Substring(file.Length - 3) == "dat"){
                mapNames.Add(file);
                Debug.Log("Added map: " + file);
            }
        }
    }

    /*
    public void SaveFile()
     {
         string destination = Application.persistentDataPath + "/save.dat";
         FileStream file;
 
         if(File.Exists(destination)) file = File.OpenWrite(destination);
         else file = File.Create(destination);
 
         GameData data = new GameData(currentScore, currentName, currentTimePlayed);
         BinaryFormatter bf = new BinaryFormatter();
         bf.Serialize(file, data);
         file.Close();
     }
 
     public void LoadFile()
     {
         string destination = Application.persistentDataPath + "/save.dat";
         FileStream file;
 
         if(File.Exists(destination)) file = File.OpenRead(destination);
         else
         {
             Debug.LogError("File not found");
             return;
         }
 
         BinaryFormatter bf = new BinaryFormatter();
         GameData data = (GameData) bf.Deserialize(file);
         file.Close();
 
         currentScore = data.score;
         currentName = data.name;
         currentTimePlayed = data.timePlayed;
 
         Debug.Log(data.name);
         Debug.Log(data.score);
         Debug.Log(data.timePlayed);
    }
    */
}
