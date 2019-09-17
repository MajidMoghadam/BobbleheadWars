using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 50.0f;
    //reference to character controller of space marine
    private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        //get the onle and only character controller component in the game
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //store the movement direction in the x, z plane
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
        //Simple move is a built in function of character controller, move in specified direction
        characterController.SimpleMove(moveDirection * moveSpeed);
    }
}
