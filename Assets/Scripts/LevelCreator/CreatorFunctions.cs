using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEditor;

public class CreatorFunctions : MonoBehaviour
{
    public enum toolSelection {Selector = 0, Paint = 1};
    public enum toolSecondary {Point = 0,Free = 1, Rect = 2};

    private toolSelection selectedTool = 0;
    private toolSecondary selectedSecondaryTool = 0;

[SerializeField] private LayerMask m_WhatIsGround;
    public Text descriptionText;
    GameObject signMessageObject;
    public GameObject signMessageBox;
    public GameObject playerMenuBox;
    public GameObject rectBox;      //obiekt kwadratu do wizualizacji zaznaczania narzędziem kwadratu
    public GameObject warningSign;

    [Header("Save/Load related")]
    public MapManager mapManager;
    public GameObject saveNameBox;
    public GameObject saveNameInputBox;
    public GameObject loadListContent;
    public GameObject buttonPrefab;
    public GameObject loadPanel;
    string currentMapName;

    [Header("TileMaps")]
    public Tilemap selectionMap;    //mapa do wyświetlania zaznaczonego klocka
    public Tilemap previewMap;      //mapa do wyświetlania podglądu wybranego klocka
    Tilemap selectedMap;            //aktualnie wybrana mapa na której następuje malowanie
    public Tilemap foregroundMap;   //mapa główna
    public Tilemap backgroundMap;   //mapa tła

    [Header("Tiles")]
    public TileBase selectionTile;  //klocek zaznaczenia
    public TileBase previewTile;    //klocek podglądu do trybiu wybierania
    public TileBase selectedTile;   //klocek który jest aktualnie wybrany (do malowania lub podglądu)
    public TileBase lastUsedTile;   //ostatnio wybrany klocek do malowania
    public TileBase defaultTile;    //podstawowo wybierany klocek

    [Header("Buttons")]
    public List<GameObject> tileButtons;
    public List<GameObject> specialTileButtons;
    public GameObject foreGroundButton;
    public GameObject backGroundButton;
    public GameObject selectorToolButton;
    public GameObject paintToolButton;
    public GameObject pointToolButton;
    public GameObject rectToolButton;
    public GameObject freeToolButton;
    public int activeTileButton = 0;    //zmienna określająca który przycisk klocka był jest aktywny
    public int basicTileAmount;
    public GameObject saveButton;

    Vector3Int startRectBox;        //pozycja początku kwadratu zaznaczenia
    Vector3Int endRectBox;          //pozycja końca kwadratu zaznaczenia
    Vector3 point;

    //zmienne danych klocków specjalnych
    Vector3Int? playerPosition;
    Vector3Int? exitPosition;
    Vector3Int? orbPosition;
    Vector3Int? bootsPosition;
    Vector3Int? glovePosition;
    Vector3Int? featherPosition;

    // Start is called before the first frame update
    void Start()
    {
        //TODO: ustawić wartości startowe
        selectedMap = foregroundMap;
        selectedTile = defaultTile;
        activeTileButton = 0;
        ChangeTool(0);
    }

    // Update is called once per frame
    void Update()
    {
        point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mapPoint = selectedMap.WorldToCell(point);

        if(!MenuUtils.IsPointerOverUIObject()){
            MouseOnMap(mapPoint);
        }

        if(playerPosition == null || exitPosition == null){
            saveButton.GetComponent<Button>().interactable = false;
            warningSign.SetActive(true);
        }else{
            saveButton.GetComponent<Button>().interactable = true;
            warningSign.SetActive(false);
        }
    }

    //------------------------------------------------------------------------------------
    //Funkcje operacji na mapach kafelków ------------------------------------------------
    //------------------------------------------------------------------------------------

    //funkcjonalność kreatora na mapie
    void MouseOnMap(Vector3Int mapPoint){

        PreviewSelected(mapPoint);

        if(Input.GetMouseButtonDown(0)){
            ClearSelection();
            if(selectedTool == toolSelection.Selector){
                SelectTile(mapPoint);
            }else{
                if(selectedSecondaryTool == toolSecondary.Point){
                    ModifyTiles(selectedMap,selectedTile,mapPoint);
                }
                if(selectedSecondaryTool == toolSecondary.Rect){
                    ModifyTilesRectStart(selectedTile,mapPoint);
                }
            }
        }else
        if(Input.GetMouseButtonDown(1)){
            ClearSelection();
            if(selectedSecondaryTool == toolSecondary.Point){
                ModifyTiles(selectedMap,null,mapPoint);
            }
            if(selectedSecondaryTool == toolSecondary.Rect){
                ModifyTilesRectStart(null,mapPoint);
            }
        }

        if(Input.GetMouseButton(0)){
            if(selectedTool == toolSelection.Paint){
                if(selectedSecondaryTool == toolSecondary.Free){
                    ModifyTiles(selectedMap,selectedTile,mapPoint);
                }
                if(selectedSecondaryTool == toolSecondary.Rect){
                    ModifyTilesRectUpdate(mapPoint);
                }
            }
        }else
        if(Input.GetMouseButton(1)){
            if(selectedSecondaryTool == toolSecondary.Free){
                ModifyTiles(selectedMap,null,mapPoint);
            }
            if(selectedSecondaryTool == toolSecondary.Rect){
                ModifyTilesRectUpdate(mapPoint);
            }
        }

        if(Input.GetMouseButtonUp(0)){
            if(selectedTool == toolSelection.Paint){
                if(selectedSecondaryTool == toolSecondary.Rect){
                    ModifyTilesRectEnd(selectedMap,selectedTile,mapPoint);
                }
            }
        }else
        if(Input.GetMouseButtonUp(1)){
            if(selectedSecondaryTool == toolSecondary.Rect){
                ModifyTilesRectEnd(selectedMap,null,mapPoint);
            }
        }
    }

    //Wyświetlanie podglądu wybranego klocka na mapie podglądu
    void PreviewSelected(Vector3Int mapPoint){
        previewMap.ClearAllTiles();
        previewMap.SetTile(mapPoint, selectedTile);
    }

    //Zaznaczenie klocka i czynności z tym związane
    void SelectTile(Vector3Int mapPoint){
        selectionMap.SetTile(mapPoint, selectionTile);
        if(selectedMap.GetTile(mapPoint) != null)
        switch(selectedMap.GetTile(mapPoint).name){
            case "Player":
                //playerMenuBox.SetActive(true);
            break;
            case "Sign":
                signMessageBox.SetActive(true);
                Vector2 pos = new Vector2(mapPoint.x+0.5f,mapPoint.y+0.5f);
                Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, 0.5f);
                if(colliders.Length > 0){
                    signMessageObject = colliders[0].gameObject;
                    signMessageBox.GetComponent<TMP_InputField>().text = signMessageObject.GetComponent<Text>().text;
                }else{
                    Debug.Log("Failed to get sign message object.");
                }
            break;
        }
    }

    //Działania związane z odznaczeniem wyboru
    void ClearSelection(){
        selectionMap.ClearAllTiles();
        signMessageObject = null;
        signMessageBox.SetActive(false);
        //playerMenuBox.SetActive(false);
    }

    //Dodawanie lub usuwanie klocków z mapy zależne od narzędzia
    void ModifyTiles(Tilemap map, TileBase tile, Vector3Int mapPoint){
        RemoveTile(map,mapPoint);
        if(tile != null){
            PaintTile(map,tile,mapPoint);
        }
    }
    void ModifyTilesRectStart(TileBase tile, Vector3Int mapPoint){
        rectBox.SetActive(true);
        startRectBox = new Vector3Int(mapPoint.x,mapPoint.y,0);
        rectBox.transform.position = new Vector3(point.x,point.y,0f);
        if(tile != null){
            rectBox.GetComponent<SpriteRenderer>().color = new Color(0f, 0.5f, 1f, 0.5f);
        }else{
            rectBox.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.5f);
        }
    }
    void ModifyTilesRectUpdate(Vector3Int mapPoint){
        rectBox.transform.localScale = new Vector3(point.x - rectBox.transform.position.x,point.y - rectBox.transform.position.y, 1f);
    }
    void ModifyTilesRectEnd(Tilemap map, TileBase tile, Vector3Int mapPoint){
        rectBox.SetActive(false);

        if(mapPoint.x < startRectBox.x){
            endRectBox.x = startRectBox.x;
            startRectBox.x = mapPoint.x;
        }else{
            endRectBox.x = mapPoint.x;
        }
         if(mapPoint.y < startRectBox.y){
            endRectBox.y = startRectBox.y;
            startRectBox.y = mapPoint.y;
        }else{
            endRectBox.y = mapPoint.y;
        }

        FillTiles(map, tile, startRectBox,endRectBox);
    }

    void RemoveTile(Tilemap map, Vector3Int mapPoint){
        ModifySpecialTile(map.GetTile(mapPoint), null);
        if(map.GetTile(mapPoint) != null)
        if(map.GetTile(mapPoint).name == "Sign"){
            RemoveSign(mapPoint);
        }
        map.SetTile(mapPoint,null);
    }
    void PaintTile(Tilemap map, TileBase tile, Vector3Int mapPoint){
        ModifySpecialTile(tile, mapPoint);
        map.SetTile(mapPoint,tile);
    }

    //Funkcje z dodatkowymi operacjami zależnymi od rodzaju specjalnego klocka
    void ModifySpecialTile(TileBase tile, Vector3Int? mapPoint){
        if(tile != null)
        switch(tile.name){
            case "Player": 
                ModifySpecial(ref playerPosition, mapPoint);
            break;
            case "Orb":
                ModifySpecial(ref orbPosition, mapPoint);
            break;
            case "Glove":
                ModifySpecial(ref glovePosition, mapPoint);
            break;
            case "Feather":
                ModifySpecial(ref featherPosition, mapPoint);
            break;
            case "Boots":
                ModifySpecial(ref bootsPosition, mapPoint);
            break;
            case "FinalDoor":
                ModifySpecial(ref exitPosition, mapPoint);
            break;
        }
    }
    void ModifySpecial(ref Vector3Int? lastPosition,Vector3Int? mapPoint){
        if(lastPosition != null){
            foregroundMap.SetTile((Vector3Int)lastPosition,null);
            lastPosition = null;
        }
        if(mapPoint != null){
            lastPosition = mapPoint;
        }
    }
    void RemoveSign(Vector3Int mapPoint){
        Vector2 pos = new Vector2(mapPoint.x+0.5f,mapPoint.y+0.5f);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, 0.5f);
        if(colliders.Length > 0){
            Destroy(colliders[0].gameObject);
        }else{
            Debug.Log("Failed to get sign message object.");
        }
    }

    void FillTiles(Tilemap map, TileBase tile, Vector3Int start, Vector3Int end){
        if(tile != null){
            for(int i=start.x;i<=end.x;i++){
                for(int j=start.y;j<=end.y;j++){
                    RemoveTile(map, new Vector3Int(i,j,0));
                    PaintTile(map,tile,new Vector3Int(i,j,0));
                }
            }
        }else{
            for(int i=start.x;i<=end.x;i++){
                for(int j=start.y;j<=end.y;j++){
                    RemoveTile(map, new Vector3Int(i,j,0));
                }
            }
        }
    }

    public void ChangeSignMessage(String newMessage){
        signMessageObject.GetComponent<Text>().text = newMessage;
        Debug.Log("Text changed to: " + newMessage);
    }

    public void SetPlayerPosition(Vector3Int? newPos){
        playerPosition = newPos;
    }
    public void SetExitPosition(Vector3Int? newPos){
        exitPosition = newPos;
    }
    public void SetBootsPosition(Vector3Int? newPos){
        bootsPosition = newPos;
    }
    public void SetFeatherPosition(Vector3Int? newPos){
        featherPosition = newPos;
    }
    public void SetGlovePosition(Vector3Int? newPos){
        glovePosition = newPos;
    }
    public void SetOrbPosition(Vector3Int? newPos){
        orbPosition = newPos;
    }

    //------------------------------------------------------------------------------------
    //Funkcje operacji na przyciskach i UI -----------------------------------------------
    //------------------------------------------------------------------------------------

    public void ChangeTool(int tool){
        if(tool == (int)toolSelection.Paint){
            selectedTool = toolSelection.Paint;
            selectedTile = lastUsedTile;
        }else{
            lastUsedTile = selectedTile;
            selectedTile = previewTile;
            selectedTool = toolSelection.Selector;
            selectedSecondaryTool = toolSecondary.Point;
            selectedMap = foregroundMap;
        }
        UpdateButtonState();
    }

    public void ChangeSecondaryTool(int tool){
        switch(tool){
            case (int) toolSecondary.Point:
            selectedSecondaryTool = toolSecondary.Point;
            break;
            case (int) toolSecondary.Free:
            selectedSecondaryTool = toolSecondary.Free;
            break;
            case (int) toolSecondary.Rect:
            selectedSecondaryTool = toolSecondary.Rect;
            break;
        }
        UpdateButtonState();
    }

    public void ChangeSelectedTile(TileBase tile){
        selectedTile = tile;
        lastUsedTile = selectedTile;
        if(activeTileButton > basicTileAmount-1){
            ChangeSecondaryTool(0);
        }
        ChangeTool(1);
    }

    public void ChangeAvtiveTileButton(int number){
        activeTileButton = number;
    }

    public void ChangePlane(Tilemap plane){
        selectedMap = plane;
        if(selectedMap == backgroundMap){
            if(activeTileButton > basicTileAmount-1){
                activeTileButton = 0;
                selectedTile = defaultTile;
            }
        }
        UpdateButtonState();
    }

    void UpdateButtonState(){
        if(selectedTool == toolSelection.Selector){
            selectorToolButton.GetComponent<Button>().interactable = false;
            paintToolButton.GetComponent<Button>().interactable = true;
            pointToolButton.GetComponent<Button>().interactable = false;
            freeToolButton.GetComponent<Button>().interactable = false;
            rectToolButton.GetComponent<Button>().interactable = false;
            foreGroundButton.GetComponent<Button>().interactable = false;
            backGroundButton.GetComponent<Button>().interactable = false;
            foreach(GameObject button in tileButtons){
                button.GetComponent<Button>().interactable = true;
            }
            foreach(GameObject button in specialTileButtons){
                button.GetComponent<Button>().interactable = true;
            }
        }
        if(selectedTool == toolSelection.Paint){
            selectorToolButton.GetComponent<Button>().interactable = true;
            paintToolButton.GetComponent<Button>().interactable = false;
            if(selectedMap == foregroundMap){
                foreGroundButton.GetComponent<Button>().interactable = false;
                backGroundButton.GetComponent<Button>().interactable = true;
                foreach(GameObject button in tileButtons){
                    button.GetComponent<Button>().interactable = true;
                }
                foreach(GameObject button in specialTileButtons){
                    button.GetComponent<Button>().interactable = true;
                }
            }else{
                foreGroundButton.GetComponent<Button>().interactable = true;
                backGroundButton.GetComponent<Button>().interactable = false;
                foreach(GameObject button in tileButtons){
                    button.GetComponent<Button>().interactable = true;
                }
                foreach(GameObject button in specialTileButtons){
                    button.GetComponent<Button>().interactable = false;
                }
            }
            if(selectedSecondaryTool == toolSecondary.Point){
                pointToolButton.GetComponent<Button>().interactable = false;
                freeToolButton.GetComponent<Button>().interactable = true;
                rectToolButton.GetComponent<Button>().interactable = true;
            }else if(selectedSecondaryTool == toolSecondary.Free){
                pointToolButton.GetComponent<Button>().interactable = true;
                freeToolButton.GetComponent<Button>().interactable = false;
                rectToolButton.GetComponent<Button>().interactable = true;
            }else{
                pointToolButton.GetComponent<Button>().interactable = true;
                freeToolButton.GetComponent<Button>().interactable = true;
                rectToolButton.GetComponent<Button>().interactable = false;
            }
            if(activeTileButton > basicTileAmount-1){
                pointToolButton.GetComponent<Button>().interactable = false;
                freeToolButton.GetComponent<Button>().interactable = false;
                rectToolButton.GetComponent<Button>().interactable = false;
                specialTileButtons[activeTileButton-basicTileAmount].GetComponent<Button>().interactable = false;
            }else{
                tileButtons[activeTileButton].GetComponent<Button>().interactable = false;
            }
        }
    }

    public void SetDescription(string description){
        descriptionText.text = description;
    }

    //------------------------------------------------------------------------------------
    //Funkcje związane z zapisem i odczytem ----------------------------------------------
    //------------------------------------------------------------------------------------

    public void SaveButton(){
        string mapName = saveNameBox.GetComponent<Text>().text;
        currentMapName = mapName;
        mapManager.SaveMap(mapName);
    }

    public void RefreshMapList(){
        foreach (Transform child in loadListContent.transform) {
            GameObject.Destroy(child.gameObject);
        }
        List<string> mapList = mapManager.GetMapList();

        foreach(string map in mapList){
            GameObject tempObj = Instantiate(buttonPrefab);
            tempObj.transform.SetParent(loadListContent.transform);
            tempObj.GetComponentInChildren<Text>().text = map;
            tempObj.GetComponent<Button>().onClick.AddListener(delegate{LoadMap(map);});
        }
    }

    void LoadMap(string name){
        mapManager.LoadMap(name);
        currentMapName = name;
        saveNameInputBox.GetComponent<InputField>().text = name;
        loadPanel.SetActive(false);
    }

}
