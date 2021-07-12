using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public string sceneToLoad = "TestScene";
    public string editorScene = "LevelCreator";
    public string gameScene;

    public GameObject listButtonPrefab;
    public GameObject customMapList;
    public GameObject basicMapList;
    public MenuMapManager mapManager;
    public GameObject musicBox;

    AudioSource buttonSound;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] sounds = musicBox.GetComponents<AudioSource>();
        buttonSound = sounds[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGame ()
	{
		SceneManager.LoadScene(sceneToLoad);
	}

    public void LoadCreator ()
	{
		SceneManager.LoadScene(editorScene);
	}

    public void ExitGame(){
        Application.Quit();
    }

    public void RefreshMapLists(){
        PlayButtonSound();
        foreach (Transform child in basicMapList.transform) {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in customMapList.transform) {
            GameObject.Destroy(child.gameObject);
        }
        Text [] tempText;

        List<string> mapList = mapManager.GetMapList("Maps");
        foreach(string map in mapList){
            GameObject tempObj = Instantiate(listButtonPrefab);
            int[] coins = mapManager.GetCoinInfo(map,"Maps");
            int time = mapManager.GetTimeInfo(map,"Maps");
            tempObj.transform.SetParent(basicMapList.transform);
            tempText = tempObj.GetComponentsInChildren<Text>();
            tempText[0].text = map;
            if((time%60) < 10)
            tempText[1].text = (time/60) + ":0" + (time%60);
            else
            tempText[1].text = (time/60) + ":" + (time%60);
            tempObj.GetComponentInChildren<TextMeshProUGUI>().text = coins[0].ToString() + "\n" + coins[1].ToString();
            tempObj.GetComponent<Button>().onClick.AddListener(delegate{LoadLevel("Maps",map);});
        }
        mapList.Clear();
        mapList = mapManager.GetMapList("CustomMaps");
        foreach(string map in mapList){
            GameObject tempObj = Instantiate(listButtonPrefab);
            int[] coins = mapManager.GetCoinInfo(map,"CustomMaps");
            int time = mapManager.GetTimeInfo(map,"CustomMaps");
            tempObj.transform.SetParent(customMapList.transform);
            tempText = tempObj.GetComponentsInChildren<Text>();
            tempText[0].text = map;
            if((time%60) < 10)
            tempText[1].text = (time/60) + ":0" + (time%60);
            else
            tempText[1].text = (time/60) + ":" + (time%60);
            tempObj.GetComponentInChildren<TextMeshProUGUI>().text = coins[0].ToString() + "\n" + coins[1].ToString();
            tempObj.GetComponent<Button>().onClick.AddListener(delegate{LoadLevel("CustomMaps",map);});
        }
    }

    public void LoadLevel(string folder, string map){
        GameManager.ChangeCurrentMap(folder, map);
        SceneManager.LoadScene(gameScene);
    }

    public void PlayButtonSound(){
        buttonSound.Play();
    }
}
