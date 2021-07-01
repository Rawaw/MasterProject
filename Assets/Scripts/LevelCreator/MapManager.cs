using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    Vector2 mapStart;
    Vector2 mapEnd;
    int signCount;
    List<MapUtils.sign> signs;

    public CreatorFunctions creatorFunctions;

    public Tilemap foreMap;
    public Tilemap backMap;
    [Header("Tiles")]
    public TileBase grassTile;
    public TileBase forestTile;
    public TileBase rockTile;
    public TileBase spikesTile;
    public TileBase waterTile;
    public TileBase bootsTile;
    public TileBase gloveTile;
    public TileBase featherTile;
    public TileBase orbTile;
    public TileBase playerTile;
    public TileBase exitTile;
    public TileBase signTile;
    public TileBase checkpointTile;

    string currentMapName = "NoName";
    MapData currentMap;

    int timer;

    void Start()
    {
        
    }

    void Update()
    {
        if(timer > 0){
            if(timer < 2){
                SetUpSigns(currentMap.GetSignsList());
            }
            timer--;
        }
    }

    //--------------------------------------------------------------------------
    //Wczytywanie --------------------------------------------------------------
    //--------------------------------------------------------------------------

    public void LoadMap(string name){
        currentMap = MapFileManager.LoadMap("CustomMaps", name);
        if(currentMapName != null){
            foreMap.ClearAllTiles();
            backMap.ClearAllTiles();
            creatorFunctions.SetBootsPosition(null);
            creatorFunctions.SetExitPosition(null);
            creatorFunctions.SetFeatherPosition(null);
            creatorFunctions.SetGlovePosition(null);
            creatorFunctions.SetOrbPosition(null);
            creatorFunctions.SetPlayerPosition(null);

            Debug.Log("Loading map" + name + " with following Map size: " + currentMap.GetMapSize());
            currentMapName = name;
            FillMap(foreMap,currentMap.GetForeMap(),currentMap.GetMapSize());
            FillMap(backMap,currentMap.GetBackMap(),currentMap.GetMapSize());
            timer = 3;
        }
    }

    void FillMap(Tilemap mapObject, int[,] map, Vector2Int size){

        for(int x = 0; x<size.x; x++){
            for(int y = 0; y<size.y; y++){
                switch(map[x,y]){
                    case 0:
                    break;
                    case 1:
                        mapObject.SetTile(new Vector3Int(x,y,0),grassTile);
                    break;
                    case 2:
                        mapObject.SetTile(new Vector3Int(x,y,0),forestTile);
                    break;
                    case 3:
                        mapObject.SetTile(new Vector3Int(x,y,0),rockTile);
                    break;
                    case 11:
                        mapObject.SetTile(new Vector3Int(x,y,0),spikesTile);
                    break;
                    case 12:
                        mapObject.SetTile(new Vector3Int(x,y,0),waterTile);
                    break;
                    case 21:
                        mapObject.SetTile(new Vector3Int(x,y,0),bootsTile);
                        creatorFunctions.SetBootsPosition(new Vector3Int(x,y,0));
                    break;
                    case 22:
                        mapObject.SetTile(new Vector3Int(x,y,0),gloveTile);
                        creatorFunctions.SetGlovePosition(new Vector3Int(x,y,0));
                    break;
                    case 23:
                        mapObject.SetTile(new Vector3Int(x,y,0),featherTile);
                        creatorFunctions.SetFeatherPosition(new Vector3Int(x,y,0));
                    break;
                    case 24:
                        mapObject.SetTile(new Vector3Int(x,y,0),orbTile);
                        creatorFunctions.SetOrbPosition(new Vector3Int(x,y,0));
                    break;
                    case 31:
                        mapObject.SetTile(new Vector3Int(x,y,0),playerTile);
                        creatorFunctions.SetPlayerPosition(new Vector3Int(x,y,0));
                    break;
                    case 32:
                        mapObject.SetTile(new Vector3Int(x,y,0),exitTile);
                        creatorFunctions.SetExitPosition(new Vector3Int(x,y,0));
                    break;
                    case 33:
                        mapObject.SetTile(new Vector3Int(x,y,0),signTile);
                    break;
                    case 34:
                        mapObject.SetTile(new Vector3Int(x,y,0),checkpointTile);
                    break;
                }
            }
        }

    }

    void SetUpSigns(MapUtils.sign[] signs){
        foreach(MapUtils.sign sign in signs){
            Vector2 pos = new Vector2(sign.x+0.5f,sign.y+0.5f);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, 0.5f);
            if(colliders.Length > 0){
                GameObject signMessageObject = colliders[0].gameObject;
                signMessageObject.GetComponent<Text>().text = sign.message;
            }else{
                Debug.Log("Failed to get sign message object.");
            }
        }
    }

    //--------------------------------------------------------------------------
    //Zapisywanie --------------------------------------------------------------
    //--------------------------------------------------------------------------

    public void SaveMap(string name){
        signCount = 0;
        signs = new List<MapUtils.sign>();
        currentMap = new MapData();
        
        currentMap.SetMapName(name);
        currentMap.SetMapSize(GetCurrentMapSize());
        currentMap.SetBackMap(GetCurrentMap(backMap));
        currentMap.SetForeMap(GetCurrentMap(foreMap));
        currentMap.SetSignAmount(signCount);
        currentMap.SetSigns(GetCurrentSigns());

        MapFileManager.SaveMap(currentMap);
    }

    Vector2Int GetCurrentMapSize(){
        Bounds foreBounds = foreMap.localBounds;
        Bounds backBounds = backMap.localBounds;

        if(foreBounds.min.x > backBounds.min.x){
            mapStart.x = backBounds.min.x;
        }else{
            mapStart.x = foreBounds.min.x;
        }
        if(foreBounds.min.y > backBounds.min.y){
            mapStart.y = backBounds.min.y;
        }else{
            mapStart.y = foreBounds.min.y;
        }

        if(foreBounds.max.x < backBounds.max.x){
            mapEnd.x = backBounds.max.x;
        }else{
            mapEnd.x = foreBounds.max.x;
        }
        if(foreBounds.max.y < backBounds.max.y){
            mapEnd.y = backBounds.max.y;
        }else{
            mapEnd.y = foreBounds.max.y;
        }

        Vector2Int mapSize = new Vector2Int();
        mapSize.x = (int) (mapEnd.x - mapStart.x);
        mapSize.y = (int) (mapEnd.y - mapStart.y);

        Debug.Log("MapSize calcs: foreBoundsMin: " + foreBounds.min + " foreMax: " + foreBounds.max + " backBoundsMin: " + backBounds.min + " backMax: " + backBounds.max);
        Debug.Log(" mapStart: " + mapStart + " mapEnd: " + mapEnd + " mapSize: " + mapSize);

        return mapSize;
    }

    int[,] GetCurrentMap(Tilemap fromMap){
        TileBase tempTile;
        int x = (int) (mapEnd.x - mapStart.x);
        int y = (int) (mapEnd.y - mapStart.y);
        int[,] map = new int[x,y];

        for(int i = 0; i < x; i++){
            for(int j = 0; j < y; j++){
                tempTile = fromMap.GetTile(new Vector3Int(i+(int)mapStart.x,j+(int)mapStart.y,0));
                map[i,j] = MapUtils.GetTileId(tempTile);
                if(map[i,j] == 99){
                    Debug.Log("Unknown tile: " + tempTile.name);
                }
                if(map[i,j] == 33){
                    signCount++;
                    signs.Add(GetSignFromPosition(i+(int)mapStart.x,j+(int)mapStart.y));
                }
            }
        }

        return map;
    }

    MapUtils.sign[] GetCurrentSigns(){
        MapUtils.sign[] currentSigns = new MapUtils.sign[signCount];

        for(int i = 0; i<signCount; i++){
            currentSigns[i] = signs[i];
        }

        return currentSigns;
    }

    MapUtils.sign GetSignFromPosition(int x, int y){
        MapUtils.sign tempSign = new MapUtils.sign();

        Vector2 pos = new Vector2(x+0.5f,y+0.5f);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, 0.5f);
        if(colliders.Length > 0){
            GameObject signMessageObject = colliders[0].gameObject;
            tempSign.message = signMessageObject.GetComponent<Text>().text;
        }else{
            Debug.Log("Failed to get sign message object.");
        }

        tempSign.x = x-(int)mapStart.x;
        tempSign.y = y-(int)mapStart.y;

        return tempSign;
    }

    public List<string> GetMapList(){
        return MapFileManager.LoadMapList("CustomMaps");
    }
}
