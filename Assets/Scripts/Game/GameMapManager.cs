using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameMapManager : MonoBehaviour
{
    public GameManager gameManager;

    [Header("TileMaps")]
    public Tilemap groundMap;
    public Tilemap backGroundMap;
    public Tilemap waterMap;
    public Tilemap spikesMap;
    public Tilemap triggerMap;
    public Tilemap itemMap;

    [Header("Tiles")]
    public TileBase grassTile;
    public TileBase forestTile;
    public TileBase rockTile;
    public TileBase spikesTile;
    public TileBase waterTile;
    public TileBase exitTile;
    public TileBase checkpointTile;
    public TileBase coinTile;

    [Header("Objects")]
    public GameObject boots;
    public GameObject gloves;
    public GameObject feather;
    public GameObject orb;
    public GameObject signObject;

    //int timer = 0;
    //MapData currentMap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if(timer > 0){
            if(timer < 2){
                SetUpSigns(currentMap.GetSignsList());
            }
            timer--;
        }*/
    }

    public void LoadLevel(string folder, string mapName){
        MapData currentMap = MapFileManager.LoadMap(folder, mapName);
        if(currentMap.GetMapName() != null){
            groundMap.ClearAllTiles();
            backGroundMap.ClearAllTiles();
            waterMap.ClearAllTiles();
            spikesMap.ClearAllTiles();
            triggerMap.ClearAllTiles();
            itemMap.ClearAllTiles();

            FillMap(false,currentMap.GetForeMap(),currentMap.GetMapSize());
            FillMap(true,currentMap.GetBackMap(),currentMap.GetMapSize());
            SetUpSigns(currentMap.GetSignsList());
            gameManager.SetCoinsToCollect(currentMap.GetMaxCoins());
            //timer = 3;
        }
    }

    void FillMap(bool isBackground, int[,] map, Vector2Int size){
        GameObject tempObject;
        for(int x = 0; x<size.x; x++){
            for(int y = 0; y<size.y; y++){
                switch(map[x,y]){
                    case 0:
                    break;
                    case 1:
                        if(isBackground)
                            backGroundMap.SetTile(new Vector3Int(x,y,0),grassTile);
                        else
                            groundMap.SetTile(new Vector3Int(x,y,0),grassTile);
                    break;
                    case 2:
                        if(isBackground)
                            backGroundMap.SetTile(new Vector3Int(x,y,0),forestTile);
                        else
                            groundMap.SetTile(new Vector3Int(x,y,0),forestTile);
                    break;
                    case 3:
                        if(isBackground)
                            backGroundMap.SetTile(new Vector3Int(x,y,0),rockTile);
                        else
                            groundMap.SetTile(new Vector3Int(x,y,0),rockTile);
                    break;
                    case 11:
                        if(isBackground)
                            backGroundMap.SetTile(new Vector3Int(x,y,0),spikesTile);
                        else
                            spikesMap.SetTile(new Vector3Int(x,y,0),spikesTile);
                    break;
                    case 12:
                        if(isBackground)
                            backGroundMap.SetTile(new Vector3Int(x,y,0),waterTile);
                        else
                            waterMap.SetTile(new Vector3Int(x,y,0),waterTile);
                    break;
                    case 21:
                        tempObject = Instantiate(boots,new Vector3(x+0.5f,y+0.5f,0f),Quaternion.identity);
                        gameManager.SetBootsObject(tempObject);
                    break;
                    case 22:
                        tempObject = Instantiate(gloves,new Vector3(x+0.5f,y+0.5f,0f),Quaternion.identity);
                        gameManager.SetGloveObject(tempObject);
                    break;
                    case 23:
                        tempObject = Instantiate(feather,new Vector3(x+0.5f,y+0.5f,0f),Quaternion.identity);
                        gameManager.SetFeatherObject(tempObject);
                    break;
                    case 24:
                        tempObject = Instantiate(orb,new Vector3(x+0.5f,y+0.5f,0f),Quaternion.identity);
                        gameManager.SetOrbObject(tempObject);
                    break;
                    case 25:
                        triggerMap.SetTile(new Vector3Int(x,y,0),coinTile);
                    break;
                    case 31:
                        gameManager.SetPlayerPosition(x,y);
                    break;
                    case 32:
                        triggerMap.SetTile(new Vector3Int(x,y,0),exitTile);
                    break;
                    case 33:
                        //triggerMap.SetTile(new Vector3Int(x,y,0),signTile);
                    break;
                    case 34:
                        triggerMap.SetTile(new Vector3Int(x,y,0),checkpointTile);
                    break;
                }
            }
        }

    }

    void SetUpSigns(MapUtils.sign[] signs){
        GameObject tempObject;
        foreach(MapUtils.sign sign in signs){
            tempObject = Instantiate(signObject,new Vector3(sign.x+0.5f,sign.y+0.3f,0f),Quaternion.identity);
            tempObject.GetComponent<SignScript>().text = sign.message;
        }
    }

    public void UpdateCollectedCoins(string folder, string mapName, int coins){
        MapData mapData = MapFileManager.LoadMap(folder, mapName);
        int coinRecord = mapData.GetCollectedCoins();
        if(coinRecord < coins){
            mapData.SetCollectedCoins(coins);
        }
        MapFileManager.SaveMap(folder,mapData);
    }

    public void UpdateBestTime(string folder, string mapName, int time){
        MapData mapData = MapFileManager.LoadMap(folder, mapName);
        int timeRecord = mapData.GetBestTime();
        if(timeRecord > time){
            mapData.SetMapTime(time);
        }
        MapFileManager.SaveMap(folder,mapData);
    }

}
