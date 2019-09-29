﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 50.0f;
    //reference to character controller of space marine
    private CharacterController characterController;
    //reference to head for bobbling
    public Rigidbody head;
    //used in raycasting below, lets you indicate what layer the ray should hit
    public LayerMask layerMask;
    private Vector3 currentLookTarget = Vector3.zero;

    public Animator bodyAnimator;

    public float[] hitForce;
    public float timeBetweenHits = 2.5f;
    private bool isHit = false;
    private float timeSinceHit = 0;
    private int hitNumber = -1;

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

        if (isHit)
        {
            timeSinceHit += Time.deltaTime;
            if (timeSinceHit > timeBetweenHits)
            {
                isHit = false;
                timeSinceHit = 0;
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
        if (moveDirection == Vector3.zero)
        {
            bodyAnimator.SetBool("IsMoving", false);
        }
        else
        {
            //bobbles the head 
            head.AddForce(transform.right * 150, ForceMode.Acceleration);
            bodyAnimator.SetBool("IsMoving", true);
        }

        RaycastHit hit; //create an empty Raycast object to be populated
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //cast a ray form camera to mouse position
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);   //Draws ray for debugging purposes

        //Physics.Raycast draws the array, based on some parameters. Ray is 1000 units long. 
        //QueryTriggerInteraction.Ignore to ensure that other triggers are not activated
        if (Physics.Raycast(ray, out hit, 1000, layerMask,QueryTriggerInteraction.Ignore))
        {
            //hit point is where the player SHOULD be looking, if not make it so
            if (hit.point != currentLookTarget)
            {
                currentLookTarget = hit.point;
            }

            // 1
            Vector3 targetPosition = new Vector3(hit.point.x,transform.position.y, hit.point.z);
            // 2
            Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
            // 3
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10.0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Alien alien = other.gameObject.GetComponent<Alien>();
        if (alien != null)
        { // 1
            if (!isHit)
            {
                hitNumber += 1; // 2
                CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
                if (hitNumber < hitForce.Length) // 3
                {
                    cameraShake.intensity = hitForce[hitNumber];
                    cameraShake.Shake();
                }
                else
                {
                    // death todo
                }
                isHit = true; // 4
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.hurt);
            }
            alien.Die();
        }
    }
   
}
