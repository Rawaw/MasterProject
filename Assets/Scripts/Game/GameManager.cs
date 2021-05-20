using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject menuPanel;
    public string mainMenuScene;

    public GameObject player;
    public SaveData saveData;

    public GameObject waterOrb;
    public GameObject gloves;
    public GameObject boots;
    public GameObject feather;

    public Grid gameGrid;

    // Start is called before the first frame update
    void Start()
    {
        saveData.Initialize(gameGrid.WorldToCell(player.transform.position));
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
        Debug.Log("Manager saving powers as: " + powerStatus);
        saveData.SaveState(checkPointPos, powerStatus);
    }

    public void LoadSave(out int powers){
        //player.transform.position = new Vector3(saveData.GetPlayerPosition().x,saveData.GetPlayerPosition().y + 0.5f,saveData.GetPlayerPosition().z);
        player.transform.position = saveData.GetPlayerPosition();
        powers = saveData.GetPlayerPowers();
        Debug.Log("Manager got powers as: " + powers);
        loadMapSave(powers);
        return;
    }

    //
    public void loadMapSave(int powers){
        Debug.Log("Items restoring with: " + powers);
        if(powers >= 8)
            waterOrb.SetActive(false);
        else
            waterOrb.SetActive(true);

        if(powers%2 == 1)
            feather.SetActive(false);
        else
            feather.SetActive(true);

        if(powers%4 > 1)
            boots.SetActive(false);
        else
            boots.SetActive(true);
        
        if(powers%8 > 3)
            gloves.SetActive(false);
        else
            gloves.SetActive(true);
    }
}
