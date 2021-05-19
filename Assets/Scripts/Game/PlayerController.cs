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

    public Rigidbody2D rigidbody;

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
         if(other.TryGetComponent(out Tilemap map)){

            Vector3Int cellPosition = map.WorldToCell(other.ClosestPoint(transform.position));
            Debug.Log("Map object name: " + map.name + "\n Cell Pos: " + cellPosition.x + " " + cellPosition.y + " " + cellPosition.z);

            TileBase tile = map.GetTile(cellPosition);
            if(tile != null){
                Debug.Log("Tile type: " + tile.GetType() + "\n Tile name: " + tile.name);
                switch(tile.name){
                    case "CheckPoint": 
                        manager.UpdateCheckPoint(cellPosition);
                        break;
                    case "Water16x16":
                        if(!controller.canSwim)
                            killCharacter();
                        else{
                            controller.SetInWater(true);
                        }
                        break;
                }
            }else{
                Debug.Log("Tile is null");
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.TryGetComponent(out Tilemap map)){

            Vector3Int cellPosition = map.WorldToCell(other.ClosestPoint(transform.position));
            Debug.Log("Map object name: " + map.name + "\n Cell Pos: " + cellPosition.x + " " + cellPosition.y + " " + cellPosition.z);

            TileBase tile = map.GetTile(cellPosition);
            if(tile != null){
                Debug.Log("Tile type: " + tile.GetType() + "\n Tile name: " + tile.name);
                switch(tile.name){
                    case "CheckPoint": 
                        break;
                    case "Water16x16":
                        controller.SetInWater(false);
                        break;
                }
            }else{
                Debug.Log("Tile is null");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.TryGetComponent(out Tilemap map)){

            Vector3Int cellPosition = map.WorldToCell(other.ClosestPoint(transform.position));

            TileBase tile = map.GetTile(cellPosition);
            if(tile != null){
                switch(tile.name){
                    case "Water16x16":
                        controller.SetInWater(true);
                        break;
                }
            }else{
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.TryGetComponent(out Tilemap map))
        switch(map.name){
            case "GroundCollisionMap": break;
            case "TriggerMap": break;
            case "Water": break;
            case "Spikes": 
                killCharacter();
                break;
        }else{
            switch(other.collider.tag){
                case "WaterOrb": 
                    controller.SetSwim(true);
                    break;
                case "Feather":
                    controller.SetMultiJump(true);
                    break;
                case "Glove":
                    controller.SetCling(true);
                    break;
                case "Boots":
                    controller.SetDash(true);
                    break;
            }
            if(other.collider.gameObject.layer == 10){
                other.collider.gameObject.active = false;
            }
        }
        
    }

    public void OnLanding(){
        //isJumping = false;
        //isFalling = false;
    }

    public void killCharacter(){
        rigidbody.bodyType = RigidbodyType2D.Static;
        animator.SetTrigger("Dying");
        controller.SetDash(false);
    }

    public void ReviveCharacter(){
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        manager.RevivePlayer();
    }

}
