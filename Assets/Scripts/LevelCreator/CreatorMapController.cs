using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorMapController : MonoBehaviour
{
    public float speed;
    public float minZoom;
    public float maxZoom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        float zoomMod = Camera.main.orthographicSize;
        transform.Translate(xInput*Time.deltaTime*speed*zoomMod,yInput*Time.deltaTime*speed*zoomMod,0f);

        if(!MenuUtils.IsPointerOverUIObject()){
            Camera.main.orthographicSize -= Input.mouseScrollDelta.y;
        }

        if(Camera.main.orthographicSize < minZoom){
            Camera.main.orthographicSize = minZoom;
        }

        if(Camera.main.orthographicSize > maxZoom){
            Camera.main.orthographicSize = maxZoom;
        }
    }
}
