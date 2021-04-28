using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject TileMenu;

    public GameObject BasicMenu;

    public GameObject InspectMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleTileMenu(){
        TileMenu.transform.position = new Vector3(TileMenu.transform.position.x*(-1f),TileMenu.transform.position.y,TileMenu.transform.position.z);
    }

    public void ToggleBasicMenu(){
        //BasicMenu.transform.position = new Vector3(BasicMenu.transform.position.x*(-1f),BasicMenu.transform.position.y,BasicMenu.transform.position.z);
        if(BasicMenu.active){
            BasicMenu.SetActive(false);
        }else{
            BasicMenu.SetActive(true);
        }
    }

    public void ToggleInspectMenu(){
        InspectMenu.transform.position = new Vector3(InspectMenu.transform.position.x,InspectMenu.transform.position.y*(-1f),InspectMenu.transform.position.z);
    }
}
