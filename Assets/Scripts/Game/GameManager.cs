using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject menuPanel;
    public string mainMenuScene;
    public GameObject finalScreen;

    public GameObject player;
    public GameObject checkpointMarker;
    public SaveData saveData;
    public Grid gameGrid;

    [Header("Ability Settings")]
    public GameObject waterOrb;
    public GameObject gloves;
    public GameObject boots;
    public GameObject feather;

    public GameObject waterOrbUiImage;
    public GameObject glovesUiImage;
    public GameObject bootsUiImage;
    public GameObject featherUiImage;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        saveData.Initialize(gameGrid.WorldToCell(player.transform.position));
        checkpointMarker.transform.position = player.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel")){
            ToggleMenu();
        }
    }

    public void ToggleMenu(){
        if(menuPanel.active){
            menuPanel.SetActive(false);
            Time.timeScale = 1;
        }else{
            menuPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ReturnToMenu(){
        SceneManager.LoadScene(mainMenuScene);
    }

    public void UpdateCheckPoint(Vector3Int checkPointPos, int powerStatus){
        checkpointMarker.transform.position = checkPointPos + new Vector3(0.5f,0.8f,0f);
        saveData.SaveState(checkPointPos, powerStatus);
    }

    public void LoadSave(out int powers){
        player.transform.position = saveData.GetPlayerPosition();
        powers = saveData.GetPlayerPowers();
        loadMapSave(powers);
        return;
    }

    //
    void loadMapSave(int powers){
        if(powers >= 8){
            waterOrb.SetActive(false);
            waterOrbUiImage.SetActive(true);
        }
        else{
            waterOrb.SetActive(true);
            waterOrbUiImage.SetActive(false);
        }

        if(powers%2 == 1){
            feather.SetActive(false);
            featherUiImage.SetActive(true);
        }
        else{
            feather.SetActive(true);
            featherUiImage.SetActive(false);
        }

        if(powers%4 > 1){
            boots.SetActive(false);
            bootsUiImage.SetActive(true);
        }
        else{
            boots.SetActive(true);
            bootsUiImage.SetActive(false);
        }
        
        if(powers%8 > 3){
            gloves.SetActive(false);
            glovesUiImage.SetActive(true);
        }
        else{
            gloves.SetActive(true);
            glovesUiImage.SetActive(false);
        }
    }

    public void FinishLevel(){
        finalScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void updateUi(int powers){
        if(powers >= 8){
            waterOrbUiImage.SetActive(true);
        }
        else{
            waterOrbUiImage.SetActive(false);
        }

        if(powers%2 == 1){
            featherUiImage.SetActive(true);
        }
        else{
            featherUiImage.SetActive(false);
        }

        if(powers%4 > 1){
            bootsUiImage.SetActive(true);
        }
        else{
            bootsUiImage.SetActive(false);
        }
        
        if(powers%8 > 3){
            glovesUiImage.SetActive(true);
        }
        else{
            glovesUiImage.SetActive(false);
        }
    }
}
