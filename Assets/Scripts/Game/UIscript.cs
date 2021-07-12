using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIscript : MonoBehaviour
{
    public GameObject itemText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItemDecsription(string text){
        itemText.GetComponent<UnityEngine.UI.Text>().text = text;
    }
}
