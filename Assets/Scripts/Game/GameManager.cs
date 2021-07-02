using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static string currentMapName = "Tutorial";
    static string currentMapFolder = "Maps";

    public GameMapManager mapManager;

    public GameObject menuPanel;
    public string mainMenuScene;
    public GameObject finalScreen;
    public GameObject coinsText;

    public Tilemap triggerMap;
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
    public GameObject itemText;
    public Animator itemAnimator;

    int startupTimer = 5;

    int collectedCoins = 0;
    int coinsToCollect;

    // Start is called before the first frame update
    void Start()
    {
        mapManager.LoadLevel(currentMapFolder,currentMapName);

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

        if(startupTimer > 0){
            if(startupTimer == 1){
                player.transform.position = saveData.GetPlayerPosition() + new Vector3(0.5f,0.8f,0f);
            }
            startupTimer--;
        }
        coinsText.GetComponent<Text>().text = collectedCoins.ToString() + "/" + coinsToCollect.ToString();
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
        Time.timeScale=1;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void UpdateCheckPoint(Vector3Int checkPointPos, int powerStatus){
        checkpointMarker.transform.position = checkPointPos + new Vector3(0.5f,0.8f,0f);
        saveData.SaveState(checkPointPos, powerStatus);
    }

    public void LoadSave(out int powers){
        player.transform.position = saveData.GetPlayerPosition() + new Vector3(0.5f,0.8f,0f);
        powers = saveData.GetPlayerPowers();
        loadMapSave(powers);
        return;
    }

    //
    void loadMapSave(int powers){
        if(waterOrb != null){
            if(powers >= 8){
                waterOrb.SetActive(false);
                waterOrbUiImage.SetActive(true);
            }
            else{
                waterOrb.SetActive(true);
                waterOrbUiImage.SetActive(false);
            }
        }else{
            waterOrbUiImage.SetActive(false);
        }

        if(feather != null){
            if(powers%2 == 1){
                feather.SetActive(false);
                featherUiImage.SetActive(true);
            }
            else{
                feather.SetActive(true);
                featherUiImage.SetActive(false);
            }
        }else{
            featherUiImage.SetActive(false);
        }

        if(boots != null){
            if(powers%4 > 1){
                boots.SetActive(false);
                bootsUiImage.SetActive(true);
            }
            else{
                boots.SetActive(true);
                bootsUiImage.SetActive(false);
            }
        }else{
            bootsUiImage.SetActive(false);
        }
        
        if(gloves != null){
            if(powers%8 > 3){
                gloves.SetActive(false);
                glovesUiImage.SetActive(true);
            }
            else{
                gloves.SetActive(true);
                glovesUiImage.SetActive(false);
            }
        }else{
            glovesUiImage.SetActive(false);
        }
    }

    public void FinishLevel(){
        finalScreen.SetActive(true);
        Time.timeScale = 0;
        mapManager.UpdateCollectedCoins(currentMapFolder, currentMapName, collectedCoins);
    }

    public void updateUi(int powers){
        if(powers >= 8){
            if(!waterOrbUiImage.active){
                waterOrbUiImage.SetActive(true);
                itemText.GetComponent<UnityEngine.UI.Text>().text = "You found Water Orb!\n Now you can swim in water safely.";
                itemAnimator.SetTrigger("OrbCollected");
            }
        }
        else{
            waterOrbUiImage.SetActive(false);
        }

        if(powers%2 == 1){
            if(!featherUiImage.active){
                featherUiImage.SetActive(true);
                itemText.GetComponent<UnityEngine.UI.Text>().text = "You found Feather!\n It allows you to jump again in the air.";
                itemAnimator.SetTrigger("FeatherCollected");
            }
        }
        else{
            featherUiImage.SetActive(false);
        }

        if(powers%4 > 1){
            if(!bootsUiImage.active){
                bootsUiImage.SetActive(true);
                itemText.GetComponent<UnityEngine.UI.Text>().text = "You found Boots!\n Now you can dash with [Shift] key.";
                itemAnimator.SetTrigger("BootsCollected");
            }
        }
        else{
            bootsUiImage.SetActive(false);
        }
        
        if(powers%8 > 3){
            if(!glovesUiImage.active){
                glovesUiImage.SetActive(true);
                itemText.GetComponent<UnityEngine.UI.Text>().text = "You found Gloves!\n Now you can cling to walls and jump off them.";
                itemAnimator.SetTrigger("GloveCollected");
            }
        }
        else{
            glovesUiImage.SetActive(false);
        }
    }

    public void RestartGame() {
        Time.timeScale=1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }

    public static void ChangeCurrentMap(string folder, string name){
        currentMapFolder = folder;
        currentMapName = name;
    }

    public void SetPlayerPosition(float x, float y){
        player.transform.position = new Vector3(x,y,0f);
    }

    public void SetBootsObject(GameObject bootsItem){
        boots = bootsItem;
    }
    public void SetGloveObject(GameObject gloveItem){
        gloves = gloveItem;
    }
    public void SetOrbObject(GameObject orbItem){
        waterOrb = orbItem;
    }
    public void SetFeatherObject(GameObject featherItem){
        feather = featherItem;
    }

    public void CollectCoin(Vector3Int position){
        triggerMap.SetTile(position,null);
        collectedCoins++;
    }

    public void SetCoinsToCollect(int maxCoins){
        coinsToCollect = maxCoins;
    }
}
