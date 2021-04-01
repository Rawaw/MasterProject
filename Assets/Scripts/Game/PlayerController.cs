using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController2D controller;
    public float runSpeed;
    public Animator animator;

    Vector3 prevPosition;
    float horizontalMovement = 0f;
    bool jump = false;
    bool isJumping = false;
    bool isFalling = false;

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
            //isJumping = true;
        }

        

        animator.SetFloat("Speed",Mathf.Abs(horizontalMovement));
        animator.SetBool("IsJumping",isJumping);
        animator.SetBool("IsFalling",isFalling);

        
    }

    void FixedUpdate() {

        controller.Move(horizontalMovement*Time.fixedDeltaTime,false,jump);
        jump = false;

        if(transform.position.y < prevPosition.y-0.03f){
            isFalling = true;
        }else if(transform.position.y > prevPosition.y+0.03){
            isJumping = true;
            isFalling = false;
        }else {
            isFalling = false;
            isJumping = false;
        }
        prevPosition = transform.position;
    }

    public void OnLanding(){
        //isJumping = false;
        //isFalling = false;
    }
}
