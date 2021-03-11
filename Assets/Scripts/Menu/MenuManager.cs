using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string sceneToLoad = "TestScene";
    public string editorScene = "LevelCreator";

    // Start is called before the first frame update
    void Start()
    {
        
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
}
