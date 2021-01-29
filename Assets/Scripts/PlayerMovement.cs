using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   /* Handles the player's movement
   https://www.youtube.com/watch?v=mbzXIOKZurA - used for the basic movement + collisions
   
    If the player can ever take more than one step, note that this method of collision detection
    WILL miss times when the player shouldn't move. I have a solution in mind, but it's not
    efficient. (Basically checking each tile the player would pass before moving). If there's a
    better way, I don't know it
    */
   
    public float moveSpeed = 2f; // the speed that the player moves
    public Transform playerMovePoint; // this is where the player will move to each time a movement key is pressed
    [SerializeField] int step = 1; // the number of tiles the player can move with each step

    public GameObject mirrorPlayer; // the player's mirror image
    public Transform mirrorMovePoint; // this is where the mirror player will move to when the player moves

    Vector3 moveCheck; // used to cut down on repitition - could be a horizontal or vertical movement in either direction
    private float movementX; // movement in x axis
    private float movementY; // movement in y axis

    // for handling collisions
    public LayerMask obstacle;
    private float checkCircleSize = .4f;

    public void Start(){
        
        // objects were nested for the sake of organisation - need to detach from parents
        playerMovePoint.parent= null;
        mirrorMovePoint.parent = null;
    }

    public void Update(){

        movementX = Input.GetAxisRaw("Horizontal");
        movementY = Input.GetAxisRaw("Vertical");

        transform.position = Vector3.MoveTowards(transform.position, playerMovePoint.position, moveSpeed * Time.deltaTime);
        mirrorPlayer.transform.position = Vector3.MoveTowards(mirrorPlayer.transform.position, mirrorMovePoint.position, moveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, playerMovePoint.position) <= .05f && Vector3.Distance(mirrorPlayer.transform.position, mirrorMovePoint.position) <= 0.5f){
            
            if(Mathf.Abs(movementX) == 1f){
                moveCheck = new Vector3(movementX * step, 0f, 0f); // horizontal movement

                // before actually moving the points, make sure they won't collide with anything
                if(!Physics2D.OverlapCircle(playerMovePoint.position + moveCheck, checkCircleSize, obstacle) && !Physics2D.OverlapCircle(mirrorMovePoint.position - moveCheck, checkCircleSize, obstacle)){
                    playerMovePoint.position += new Vector3(movementX * step, 0f, 0f);
                    mirrorMovePoint.position -= new Vector3(movementX * step, 0f, 0f); // will move left if player moves right and visa versa
                }                
            }

            if(Mathf.Abs(movementY) == 1f){
                moveCheck = new Vector3(0f, movementY * step, 0f); // vertical movement

                if(!Physics2D.OverlapCircle(playerMovePoint.position + moveCheck, checkCircleSize, obstacle) && !Physics2D.OverlapCircle(mirrorMovePoint.position - moveCheck, checkCircleSize, obstacle)){
                    playerMovePoint.position += new Vector3(0f, movementY * step, 0f);
                    mirrorMovePoint.position -= new Vector3(0f, movementY * step, 0f); // will move up if player moves down and visa versa
                }
            }
        }
    }
}
