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

    public void UpdateCheckPoint(Vector3Int checkPointPos){
        saveData.SaveState(checkPointPos);
    }

    public void RevivePlayer(){
        player.transform.position = new Vector3(saveData.getPlayerPosition().x,saveData.getPlayerPosition().y + 0.5f,saveData.getPlayerPosition().z);
    }
}
