using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public CharacterController2D controller;
    public float runSpeed;
    public Animator animator;
    public GameManager manager;

    float horizontalMovement = 0f;
    bool jump = false;
    bool dash = false;
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

        if(Input.GetButtonDown("Dash")){
            dash = true;
            if(controller.CanDash())
                animator.SetTrigger("IsDashing");
        }

        isJumping = controller.IsJumping();
        isFalling = controller.IsFalling();
        isClinging = controller.IsClinging();

        animator.SetFloat("Speed",Mathf.Abs(horizontalMovement));
        animator.SetBool("IsJumping",isJumping);
        animator.SetBool("IsFalling",isFalling);
        animator.SetBool("IsClinging",isClinging);
    }

    void FixedUpdate() {

        controller.Move(horizontalMovement*Time.fixedDeltaTime,false);
        controller.Jump(jump);
        controller.Dash(dash);
        jump = false;
        dash = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Tilemap map = other.GetComponent<Tilemap>(); 
        Vector3Int cellPosition = map.WorldToCell(transform.position);
        string tiletype = map.GetTile(cellPosition).name;

        if(tiletype == "CheckPoint")
        manager.UpdateCheckPoint();
    }

    public void OnLanding(){
        //isJumping = false;
        //isFalling = false;
    }

}
