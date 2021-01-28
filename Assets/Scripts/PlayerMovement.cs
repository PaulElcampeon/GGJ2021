using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   /* Handles the player's movement
   https://www.youtube.com/watch?v=mbzXIOKZurA - used for the basic movement */
    public float moveSpeed = 2f; // the speed that the player moves
    public Transform playerMovePoint; // this is where the player will move to each time a movement key is pressed
    [SerializeField] int step = 1; // the number of tiles the player can move with each step

    public GameObject mirrorPlayer; // the player's mirror image
    public Transform mirrorMovePoint; // this is where the mirror player will move to when the player moves

    public void Start(){
        
        // objects were nested for the sake of organisation - need to detach from parents
        playerMovePoint.parent= null;
        mirrorMovePoint.parent = null;
    }

    public void Update(){

        transform.position = Vector3.MoveTowards(transform.position, playerMovePoint.position, moveSpeed * Time.deltaTime);
        mirrorPlayer.transform.position = Vector3.MoveTowards(mirrorPlayer.transform.position, mirrorMovePoint.position, moveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, playerMovePoint.position) <= .05f && Vector3.Distance(mirrorPlayer.transform.position, mirrorMovePoint.position) <= 0.5f){
            
            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f){
                playerMovePoint.position += new Vector3(Input.GetAxisRaw("Horizontal") * step, 0f, 0f);
                mirrorMovePoint.position -= new Vector3(Input.GetAxisRaw("Horizontal") * step, 0f, 0f); // will move left if player moves right and visa versa
            }

            if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f){
                playerMovePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical") * step, 0f);
                mirrorMovePoint.position -= new Vector3(0f, Input.GetAxisRaw("Vertical") * step, 0f); // will move up if player moves down and visa versa
            }
        }
    }
}
