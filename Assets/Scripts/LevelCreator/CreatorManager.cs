using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class CreatorManager : MonoBehaviour
{
    public Tilemap selectionMap;
    public Tilemap previewMap;
    public Tilemap foregroundMap;

    public TileBase selectionTile;
    public TileBase previewTile;

    public TileBase groundTile;

    Vector3Int startSelectionBox;
    Vector3Int endSelectionBox;

    private Vector3Int previousMousePos = new Vector3Int();
    Vector3Int holdMousePos = new Vector3Int();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int selectedPosition = selectionMap.WorldToCell(point);

        if(!MenuUtils.IsPointerOverUIObject()){
            PreviewMapFunction(selectedPosition);
            //SelectionMapFunction(selectedPosition);
            ForeGroundMapFunction(selectedPosition);
        }
        
    }

    void PreviewMapFunction(Vector3Int position){

        if (!position.Equals(previousMousePos)) {
            previewMap.SetTile(previousMousePos, null); // Remove old hoverTile
            previewMap.SetTile(position, previewTile);
            previousMousePos = position;
        }
    }

    void SelectionMapFunction(Vector3Int position){
        if(Input.GetMouseButtonDown(0)){
            if(!Input.GetKey(KeyCode.LeftControl)){
                selectionMap.ClearAllTiles();
            }
            selectionMap.SetTile(position,selectionTile);
            startSelectionBox.x = position.x;
            startSelectionBox.y = position.y;
        }

        if(Input.GetMouseButtonUp(0)){
            if(position.x < startSelectionBox.x){
                endSelectionBox.x = startSelectionBox.x;
                startSelectionBox.x = position.x;
            }else{
                endSelectionBox.x = position.x;
            }
            if(position.y < startSelectionBox.y){
                endSelectionBox.y = startSelectionBox.y;
                startSelectionBox.y = position.y;
            }else{
                endSelectionBox.y = position.y;
            }
            MapUtils.BoxFill(selectionMap,selectionTile,startSelectionBox,endSelectionBox);
        }

        if(Input.GetMouseButtonDown(1)){
            selectionMap.ClearAllTiles();
        }

    }

    void ForeGroundMapFunction(Vector3Int position){
        if(Input.GetMouseButton(0)){
            foregroundMap.SetTile(position, groundTile);
        }
        if(Input.GetMouseButton(1)){
            foregroundMap.SetTile(position, null);
        }
    }

    public void ChangeGroundTile(TileBase tile){
        groundTile = tile;
    }

    public void LoadMenu ()
	{
		SceneManager.LoadScene("Menu");
	}

}
