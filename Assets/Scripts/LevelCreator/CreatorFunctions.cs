using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CreatorFunctions : MonoBehaviour
{
    public enum toolSelection {Selector = 0, Paint = 1};
    public enum toolSecondary {Point = 0,Free = 1, Rect = 2};

    private toolSelection selectedTool = 0;
    private toolSecondary selectedSecondaryTool = 0;

    [Header("TileMaps")]
    public Tilemap selectionMap;
    public Tilemap previewMap;
    public Tilemap selectedMap;

    [Header("Tiles")]
    public TileBase selectionTile;
    public TileBase previewTile;
    public TileBase selectedTile;

    [Header("Buttons")]
    public List<GameObject> tileButtons;
    public GameObject selectorToolButton;
    public GameObject paintToolButton;
    public GameObject pointToolButton;
    public GameObject rectToolButton;
    public GameObject freeToolButton;

    Vector3Int startSelectionBox;
    Vector3Int endSelectionBox;
    Boolean foregroundMode = true;

    Vector3Int playerPosition;
    Vector3Int exitPosition;
    Vector3Int orbPosition;
    Vector3Int bootsPosition;
    Vector3Int glovePosition;
    Vector3Int featherPosition;

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
        if(!IsSpecialTile(selectedTile)){
            if(selectedTile != null)
            PaintTiles(selectedMap,selectedTile,position);
        }else{
            if(Input.GetMouseButtonDown(0))
                PaintSpecialTiles(selectedMap,selectedTile,position);
        }
        if(Input.GetMouseButton(1)){
            if(!IsSpecialTile(selectedMap.GetTile(position))){
                selectedMap.SetTile(position, null);
            }else{
                switch(selectedMap.GetTile(position).name){
                    case "Player": 
                        playerPosition = Vector3Int.zero;
                        break;
                    case "Orb":
                        orbPosition = Vector3Int.zero;
                        break;
                    case "Glove":
                        glovePosition = Vector3Int.zero;
                        break;
                    case "Feather":
                        featherPosition = Vector3Int.zero;
                        break;
                    case "Boots":
                        bootsPosition = Vector3Int.zero;
                        break;
                    case "FinalDoor":
                        exitPosition = Vector3Int.zero;
                        break;
                }
                selectedMap.SetTile(position, null);
            }
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

    void PaintSpecialTiles(Tilemap targetMap, TileBase targetTile,Vector3Int position){
        switch(selectedTile.name){
            case "Player": 
                if(playerPosition != null){
                    targetMap.SetTile(playerPosition, null);
                }
                playerPosition = position;
            break;
            case "Orb":
                if(orbPosition != null){
                    targetMap.SetTile(orbPosition, null);
                }
                orbPosition = position;
            break;
            case "Glove":
                if(glovePosition != null){
                    targetMap.SetTile(glovePosition, null);
                }
                glovePosition = position;
            break;
            case "Feather":
                if(featherPosition != null){
                    targetMap.SetTile(featherPosition, null);
                }
                featherPosition = position;
            break;
            case "Boots":
                if(bootsPosition != null){
                    targetMap.SetTile(bootsPosition, null);
                }
                bootsPosition = position;
            break;
            case "Sign":
            case "FinalDoor":
                if(exitPosition != null){
                    targetMap.SetTile(exitPosition, null);
                }
                exitPosition = position;
            break;
            case "CheckPoint":
                break;
        }
        targetMap.SetTile(position,targetTile);
    }



    public void ChangeSelectedTile(TileBase tile){
        selectedTile = tile;
        SetTileButtonsInteractable();
        ChangeTool(1);
    }

    public void ChangeTool(int selection){
        selectedTool = (toolSelection)selection;
        if(selectedTool == toolSelection.Paint){
            paintToolButton.GetComponent<Button>().interactable = false;
            selectorToolButton.GetComponent<Button>().interactable = true;
        }else{
            paintToolButton.GetComponent<Button>().interactable = true;
            selectorToolButton.GetComponent<Button>().interactable = false;
        }
    }
    public void ChangeToolSecondary(int selection){
        selectedSecondaryTool = (toolSecondary)selection;
        switch(selectedSecondaryTool){
            case toolSecondary.Point:
                pointToolButton.GetComponent<Button>().interactable = false;
                freeToolButton.GetComponent<Button>().interactable = true;
                rectToolButton.GetComponent<Button>().interactable = true;
                break;
            case toolSecondary.Free:
                pointToolButton.GetComponent<Button>().interactable = true;
                freeToolButton.GetComponent<Button>().interactable = false;
                rectToolButton.GetComponent<Button>().interactable = true;
                break;
            case toolSecondary.Rect:
                pointToolButton.GetComponent<Button>().interactable = true;
                freeToolButton.GetComponent<Button>().interactable = true;
                rectToolButton.GetComponent<Button>().interactable = false;
                break;
        }
    }

    public void ChangeSelectedTileMap(Tilemap map){
        selectedMap = map;
        if(selectedMap.tag == "Background"){
            foregroundMode = false;
            DenySpecialTileSelection();
            int i = 0;
            foreach(GameObject button in tileButtons){
                if(i > 4){
                    button.GetComponent<Button>().interactable = false;
                }
                i++;
            }
        }else{
            foregroundMode = true;
            int i = 0;
            foreach(GameObject button in tileButtons){
                if(i > 4){
                    button.GetComponent<Button>().interactable = true;
                }
                i++;
            }
        }
    }

    public void SetTileButtonsInteractable(){
        if(foregroundMode){
            foreach(GameObject button in tileButtons){
                button.GetComponent<Button>().interactable = true;
            }
        }else{
            int i = 0;
            foreach(GameObject button in tileButtons){
                if(i > 4){
                    button.GetComponent<Button>().interactable = false;
                }else{
                    button.GetComponent<Button>().interactable = true;
                }
                i++;
            }
            DenySpecialTileSelection();
        }
    }

    private void DenySpecialTileSelection(){
        if(selectedTile != null)
        switch(selectedTile.name){
            case "Player": 
            case "Orb":
            case "Glove":
            case "Feather":
            case "Boots":
            case "Sign":
            case "FinalDoor":
            case "CheckPoint":
                selectedTile = null;
                break;
        }
    }

    private bool IsSpecialTile(TileBase tile){
        if(tile != null)
        switch(tile.name){
            case "Player": 
            case "Orb":
            case "Glove":
            case "Feather":
            case "Boots":
            case "Sign":
            case "FinalDoor":
            case "CheckPoint":
                return true;
        }
        return false;
    }

}
