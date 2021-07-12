using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CoinCollector : MonoBehaviour
{
    
    public GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.TryGetComponent(out Tilemap map)){

            Vector3Int cellPosition = map.WorldToCell(other.ClosestPoint(transform.position));

            TileBase tile = map.GetTile(cellPosition);
            if(tile != null){
                switch(tile.name){
                    case "Coin":
                        manager.CollectCoin(cellPosition);
                    break;
                }
            }else{
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out Tilemap map)){

            Vector3Int cellPosition = map.WorldToCell(other.ClosestPoint(transform.position));

            TileBase tile = map.GetTile(cellPosition);
            if(tile != null){
                switch(tile.name){
                    case "Coin":
                        manager.CollectCoin(cellPosition);
                    break;
                }
            }else{
            }
        }
    }
}
