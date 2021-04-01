using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController2D controller;
    public float runSpeed;
    public Animator animator;

    float horizontalMovement = 0f;
    bool jump = false;
    bool isJumping = false;
    bool isFalling = false;
    bool isClinging = false;

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

        isJumping = controller.isJumping();
        isFalling = controller.isFalling();
        isClinging = controller.isClinging();

        animator.SetFloat("Speed",Mathf.Abs(horizontalMovement));
        animator.SetBool("IsJumping",isJumping);
        animator.SetBool("IsFalling",isFalling);
        animator.SetBool("IsClinging",isClinging);
    }

    void FixedUpdate() {

        controller.Move(horizontalMovement*Time.fixedDeltaTime,false,jump);
        jump = false;
    }

    public void OnLanding(){
        //isJumping = false;
        //isFalling = false;
    }
}
