using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController2D controller;
    public float runSpeed;

    float horizontalMovement = 0f;
    bool jump = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal")*runSpeed;

        if(Input.GetButtonDown("Jump")){
            jump = true;
        }
    }

    void FixedUpdate() {

        controller.Move(horizontalMovement*Time.fixedDeltaTime,false,jump);
        jump = false;

    }


}
