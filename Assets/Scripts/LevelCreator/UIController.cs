using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public Animator animator;

    public GameObject basicMenu;

    Boolean tilesOpen = true;
    Boolean basicOpen = false;
    Boolean inspectorOpen = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleTilesMenu(){
        if(tilesOpen){
            animator.SetTrigger("TilesClose");
            tilesOpen = false;
        }
        else{
            animator.SetTrigger("TilesOpen");
            tilesOpen = true;
        }
    }

    public void ToggleBasicMenu(){
        if(basicOpen){
            animator.SetTrigger("BasicClose");
            basicOpen = false;
        }
        else{
            animator.SetTrigger("BasicOpen");
            basicOpen = true;
        }
    }

    public void ToggleInspectorMenu(){
        if(inspectorOpen){
            animator.SetTrigger("InspectorClose");
            inspectorOpen = false;
        }
        else{
            animator.SetTrigger("InspectorOpen");
            inspectorOpen = true;
        }
    }

}
