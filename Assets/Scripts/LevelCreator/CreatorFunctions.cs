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

    //public Text descriptionText;
    //public GameObject messageInputBox;
    public GameObject rectBox;      //obiekt kwadratu do wizualizacji zaznaczania narzędziem kwadratu

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

    Vector3Int startRectBox;        //pozycja początku kwadratu zaznaczenia
    Vector3Int endRectBox;          //pozycja końca kwadratu zaznaczenia

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
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mapPoint = selectedMap.WorldToCell(point);

        if(!MenuUtils.IsPointerOverUIObject()){
            MouseOnMap(mapPoint);
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
            }
        }else
        if(Input.GetMouseButton(1)){
            if(selectedSecondaryTool == toolSecondary.Free){
                ModifyTiles(selectedMap,null,mapPoint);
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
        //TODO: wyciągnięcie danego klocka i danych z niego
        //TODO: dorobić obiekt tworzony ze znakiem zawierający treść znaku
        //TODO: utworzyć panel opcji gracza
            //jeżeli postać gracza wyświetl panel opcji gracza
            //jeżeli znak wyświetl panel opcji znaku
        selectionMap.SetTile(mapPoint, selectionTile);
        switch(selectedMap.GetTile(mapPoint).name){
            case "Player":
            break;
            case "Sign":
            break;
        }
    }

    //Działania związane z odznaczeniem wyboru
    void ClearSelection(){
        //TODO: wyczyścić przechowywany wybrany obiekt i wyłączyć specjalne panele
        selectionMap.ClearAllTiles();
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
        rectBox.transform.position = new Vector3(mapPoint.x,mapPoint.y,0f);
        if(tile != null){
            rectBox.GetComponent<SpriteRenderer>().color = new Color(0f, 0.5f, 1f, 0.5f);
        }else{
            rectBox.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.5f);
        }
    }
    void ModifyTilesRectUpdate(Tilemap map, TileBase tile, Vector3Int mapPoint){
        rectBox.transform.localScale = new Vector3(mapPoint.x - rectBox.transform.position.x,mapPoint.y - rectBox.transform.position.y, 1f);
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
            case "Sign":
                //TODO: modyfikacje na znakach (usuwanie\dodawanie\zastępowanie)
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

}
