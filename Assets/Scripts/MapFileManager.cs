using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MapFileManager : MonoBehaviour
{
    static string customMapPath = Application.persistentDataPath + "/CustomMaps/";

    static public void SaveMap(MapData map){
        if(!Directory.Exists(customMapPath))
        {    
            Directory.CreateDirectory(customMapPath);
            Debug.Log("Created directory: " + customMapPath);
        }
        string saveLocation = customMapPath + map.GetMapName() + ".dat";
        Debug.Log("Saving map at: " + saveLocation);

        FileStream file;
 
        if(File.Exists(saveLocation)) 
            file = File.OpenWrite(saveLocation);
        else 
            file = File.Create(saveLocation);
 
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, map);
        file.Close();
        Debug.Log("Map saved");
    }

    static public MapData LoadMap(string folder, string name){
        string destination = Application.persistentDataPath + "/" + folder + "/" + name + ".dat";
        Debug.Log("Loading map at: " + destination);
        FileStream file;
 
        if(File.Exists(destination)) file = File.OpenRead(destination);
        else{
            Debug.LogError("File not found");
            return null;
        }
 
        BinaryFormatter bf = new BinaryFormatter();
        MapData data = (MapData) bf.Deserialize(file);
        file.Close();
        Debug.Log("File loaded");
        return data;
    }

    static public List<string> LoadMapList(string folder){
        List<string> mapNames = new List<string>();
        string path = Application.persistentDataPath + "/" + folder;
        foreach (string file in System.IO.Directory.GetFiles(path)){
            Debug.Log("checking file: " + file);
            if(file.Substring(file.Length - 3) == "dat"){
                mapNames.Add(Path.GetFileName(file.Remove(file.Length-4)));
                Debug.Log("Added map: " + Path.GetFileName(file.Remove(file.Length-4)));
            }
        }
        return mapNames;
    }

    static public bool MapExists(string name){
        string saveLocation = Application.persistentDataPath + "/CustomMaps/" + name + ".dat";

        if(File.Exists(saveLocation)) 
            return true;
        else 
            return false;
    }
}
