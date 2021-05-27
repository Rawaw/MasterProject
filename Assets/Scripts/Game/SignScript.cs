using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignScript : MonoBehaviour
{
    public GameObject PopupBox;
    public string text;
    public float popupOffset;

    GameObject popupBox;
    Vector3 popupPosition;

    // Start is called before the first frame update
    void Start()
    {
        popupPosition = new Vector3(this.transform.position.x,this.transform.position.y+popupOffset,0f);
        popupBox = Instantiate(PopupBox, popupPosition, Quaternion.identity);
        popupBox.GetComponentInChildren<TextMeshPro>().text = text;
        popupBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        popupBox.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other) {
        popupBox.SetActive(false);
    }
}
