using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreatorFunctions : MonoBehaviour
{
    public enum toolSelection {Selector = 0, Paint = 1};
    public enum toolSecondary {Point = 0,Free = 1, Rect = 2};

    private toolSelection selectedTool = 0;
    private toolSecondary selectedSecondaryTool = 0;
    // TileMaps
    public Tilemap selectionMap;
    public Tilemap previewMap;
    public Tilemap selectedMap;

    // Tiles
    public TileBase selectionTile;
    public TileBase previewTile;
    public TileBase selectedTile;


    Vector3Int startSelectionBox;
    Vector3Int endSelectionBox;

    private Vector3Int previousMousePos = new Vector3Int();

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
            if(Input.GetMouseButtonDown(0)){
                if(!Input.GetKey(KeyCode.LeftControl)){
                    selectionMap.ClearAllTiles();
                }
            }
            PreviewMapFunction(selectedPosition);
            switch(selectedTool){
                case toolSelection.Selector:
                    SelectionMapFunction(selectedPosition);
                break;
                case toolSelection.Paint:
                    SelectedMapFunction(selectedPosition);
                break;
            }
        }
        
    }

    void PreviewMapFunction(Vector3Int position){

        if (!position.Equals(previousMousePos)) {
            previewMap.SetTile(previousMousePos, null); // Remove old hoverTile
            if(selectedTool == toolSelection.Paint){
                previewMap.SetTile(position, selectedTile);
            }else{
                previewMap.SetTile(position, previewTile);
            }
            previousMousePos = position;
        }
    }

    void SelectionMapFunction(Vector3Int position){
        PaintTiles(selectionMap,selectionTile,position);
    }

    void SelectedMapFunction(Vector3Int position){
        PaintTiles(selectedMap,selectedTile,position);
        if(Input.GetMouseButton(1)){
            selectedMap.SetTile(position, null);
        }
    }

    void PaintTiles(Tilemap targetMap, TileBase targetTile,Vector3Int position){
        switch(selectedSecondaryTool){
            case toolSecondary.Point: 
                if(Input.GetMouseButtonDown(0)){
                targetMap.SetTile(position,targetTile);
                }
            break;
            case toolSecondary.Free:
                if(Input.GetMouseButton(0)){
                    targetMap.SetTile(position,targetTile);
                }
            break;
            case toolSecondary.Rect:
                if(Input.GetMouseButtonDown(0)){
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
                    MapUtils.BoxFill(targetMap,targetTile,startSelectionBox,endSelectionBox);
                }
            break;
        }
    }

    public void ChangeSelectedTile(TileBase tile){
        selectedTile = tile;
    }

    public void ChangeTool(int selection){
        selectedTool = (toolSelection)selection;
    }
    public void ChangeToolSecondary(int selection){
        selectedSecondaryTool = (toolSecondary)selection;
    }
}
