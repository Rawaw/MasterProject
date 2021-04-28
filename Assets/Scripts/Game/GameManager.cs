using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject menuPanel;
    public string mainMenuScene;

    public GameObject checkPoint;
    public GameObject player;

    public Grid gameGrid;

    // Start is called before the first frame update
    void Start()
    {
        checkPoint.transform.position = player.transform.position;
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

    public void UpdateCheckPoint(){
        checkPoint.transform.position = player.transform.position;
    }
}
