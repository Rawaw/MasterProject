using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 startPos;
    public float speed;
    public float range;
    void Start()
    {
        startPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //float positionOffset = startPos + range*(Mathf.Sin(Time.deltaTime)) - range*0.5f;
        this.transform.position = startPos + new Vector3(0.0f, Mathf.Sin(Time.time*speed)*range, 0.0f);
        //Debug.Log("Offset: " + positionOffset);
    }
}
