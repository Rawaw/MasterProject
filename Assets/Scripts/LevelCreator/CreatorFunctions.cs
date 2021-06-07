using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class CreatorFunctions : MonoBehaviour
{
    public enum toolSelection {Selector = 0, Paint = 1};
    public enum toolSecondary {Point = 0,Free = 1, Rect = 2};

    private toolSelection selectedTool = 0;
    private toolSecondary selectedSecondaryTool = 0;

    public Text descriptionText;
    public GameObject messageInputBox;

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

    Vector3Int? playerPosition;
    Vector3Int? exitPosition;
    Vector3Int? orbPosition;
    Vector3Int? bootsPosition = null;
    Vector3Int? glovePosition;
    Vector3Int? featherPosition;
    Vector3Int? selectionPosition;

    private Vector3Int previousMousePos = new Vector3Int();
    Vector2 rectStart;

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
                selectionMap.ClearAllTiles();
                descriptionText.text = "";
                selectionPosition = Vector3Int.zero;
                messageInputBox.SetActive(false);
            }
            PreviewMapFunction(selectedPosition);
            switch(selectedTool){
                case toolSelection.Selector:
                    SelectionMapFunction(selectedPosition);
                break;
                case toolSelection.Paint:
                    SelectedMapFunction(selectedPosition);
                    if(Input.GetMouseButtonDown(0)){
                        rectStart = point;
                    }
                    if(Input.GetMouseButton(0)){
                        //EditorGUI.DrawRect(new Rect(rectStart.x, rectStart.y, rectStart.x-point.x, rectStart.y-point.y), Color.blue);
                    }
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
        if(Input.GetMouseButtonDown(1)){
            selectionMap.ClearAllTiles();
            descriptionText.text = "";
            selectionPosition = Vector3Int.zero;
            messageInputBox.SetActive(false);
        }
        if(Input.GetMouseButtonDown(0)){
            selectionMap.SetTile(position,selectionTile);
            selectionPosition = position;
            messageInputBox.SetActive(false);
            if(IsSpecialTile(selectedMap.GetTile(position))){
                SpecialTile temp = (SpecialTile)selectedMap.GetTile(position);
                descriptionText.text = temp.getDescription();
                if(temp.name == "Sign"){
                    messageInputBox.SetActive(true);
                }else{
                    messageInputBox.SetActive(false);
                }
            }
        }
    }

    void SelectedMapFunction(Vector3Int position){
        if(!IsSpecialTile(selectedTile)){
            if(selectedTile != null)
            PaintTiles(selectedMap,selectedTile,position);
        }else{
            if(Input.GetMouseButtonDown(0))
                PaintSpecialTiles(selectedMap,selectedTile,position);
        }
        if(!IsSpecialTile(selectedMap.GetTile(position))){
            RemoveTiles(selectedMap,position);
            //selectedMap.SetTile(position, null);
        }else{
            removeSpecialMark(position,selectedMap);
            RemoveTiles(selectedMap,position);
            //selectedMap.SetTile(position, null);
        }
    }

    void PaintTiles(Tilemap targetMap, TileBase targetTile,Vector3Int position){
        switch(selectedSecondaryTool){
            case toolSecondary.Point: 
                if(Input.GetMouseButtonDown(0)){
                    removeSpecialMark(position,targetMap);
                    targetMap.SetTile(position,targetTile);
                }
            break;
            case toolSecondary.Free:
                if(Input.GetMouseButton(0)){
                    removeSpecialMark(position,targetMap);
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
                    BoxFill(targetMap,targetTile,startSelectionBox,endSelectionBox);
                }
            break;
        }
    }

    void RemoveTiles(Tilemap targetMap, Vector3Int position){
        switch(selectedSecondaryTool){
            case toolSecondary.Point: 
                if(Input.GetMouseButtonDown(1)){
                    removeSpecialMark(position,targetMap);
                    targetMap.SetTile(position,null);
                }
            break;
            case toolSecondary.Free:
                if(Input.GetMouseButton(1)){
                    removeSpecialMark(position,targetMap);
                    targetMap.SetTile(position,null);
                }
            break;
            case toolSecondary.Rect:
                if(Input.GetMouseButtonDown(1)){
                    startSelectionBox.x = position.x;
                    startSelectionBox.y = position.y;
                }
        
                if(Input.GetMouseButtonUp(1)){
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
                    BoxRemove(targetMap,startSelectionBox,endSelectionBox);
                }
            break;
        }
    }

    void PaintSpecialTiles(Tilemap targetMap, TileBase targetTile,Vector3Int position){
        targetMap.SetTile(position,targetTile);
        SpecialTile temp = (SpecialTile)targetMap.GetTile(position);
        switch(targetTile.name){
            case "Player": 
                if(playerPosition != null){
                    targetMap.SetTile((Vector3Int)playerPosition, null);
                }
                playerPosition = position;
                temp.setDescription("It's a Player character. The player will be able to move with this character.");
            break;
            case "Orb":
                if(orbPosition != null){
                    targetMap.SetTile((Vector3Int)orbPosition, null);
                }
                orbPosition = position;
                temp.setDescription("It's Water Orb item, that allows moving in water safely");
            break;
            case "Glove":
                if(glovePosition != null){
                    targetMap.SetTile((Vector3Int)glovePosition, null);
                }
                glovePosition = position;
                temp.setDescription("It's a Glove item, that allows grabbing walls");
            break;
            case "Feather":
                if(featherPosition != null){
                    targetMap.SetTile((Vector3Int)featherPosition, null);
                }
                featherPosition = position;
                temp.setDescription("It's a Feather item, that gives double jump ability");
            break;
            case "Boots":
                if(bootsPosition != null){
                    targetMap.SetTile((Vector3Int)bootsPosition, null);
                }
                bootsPosition = position;
                temp.setDescription("It's a Boots item, that gives dash ability");
            break;
            case "Sign":
                temp.setDescription("It's an information sign. It shows it's message, when player stands on it");
            break;
            case "FinalDoor":
                if(exitPosition != null){
                    targetMap.SetTile((Vector3Int)exitPosition, null);
                }
                exitPosition = position;
                temp.setDescription("It's an Exit door. Once reached, level is completed");
            break;
            case "CheckPoint":
                temp.setDescription("It saves game state on reaching one. After player character's death, game is returned to latest check point");
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

    private void removeSpecialMark(Vector3Int position, Tilemap map){
        if(map.GetTile(position) != null)
        switch(map.GetTile(position).name){
                        case "Player": 
                            playerPosition = null;
                            break;
                        case "Orb":
                            orbPosition = null;
                            break;
                        case "Glove":
                            glovePosition = null;
                            break;
                        case "Feather":
                            featherPosition = null;
                            break;
                        case "Boots":
                            bootsPosition = null;
                            break;
                        case "FinalDoor":
                            exitPosition = null;
                            break;
                    }
    }

    void BoxFill(Tilemap map, TileBase tile, Vector3Int startPos, Vector3Int endPos){
        int rows = endPos.x - startPos.x;
        int columns = endPos.y - startPos.y;
        for(int i = 0; i <= rows;i++){
            for(int j = 0; j <= columns;j++){
                Vector3Int fillPos = new Vector3Int(startPos.x + i, startPos.y + j, startPos.z);
                removeSpecialMark(fillPos,map);
                map.SetTile(fillPos,tile);
            }
        }
    }

    void BoxRemove(Tilemap map, Vector3Int startPos, Vector3Int endPos){
        int rows = endPos.x - startPos.x;
        int columns = endPos.y - startPos.y;
        for(int i = 0; i <= rows;i++){
            for(int j = 0; j <= columns;j++){
                Vector3Int fillPos = new Vector3Int(startPos.x + i, startPos.y + j, startPos.z);
                removeSpecialMark(fillPos,map);
                map.SetTile(fillPos,null);
            }
        }
    }

}
