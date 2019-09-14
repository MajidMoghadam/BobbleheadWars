using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    
    public GameObject followTarget;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (followTarget != null)
        {
            //this makes the camera mount position smoothly move to where the player is over time
            transform.position = Vector3.Lerp(transform.position, 
                followTarget.transform.position, Time.deltaTime * moveSpeed);
        }
    }
}
