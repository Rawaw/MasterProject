using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorMapController : MonoBehaviour
{
    public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        transform.Translate(xInput*Time.deltaTime*speed,yInput*Time.deltaTime*speed,0f);

        Camera.main.orthographicSize -= Input.mouseScrollDelta.y;
        /*
        if(transform.position.y < Camera.main.orthographicSize){
            transform.position = new Vector3(transform.position.x, Camera.main.orthographicSize, transform.position.z);
        }*/
    }
}
