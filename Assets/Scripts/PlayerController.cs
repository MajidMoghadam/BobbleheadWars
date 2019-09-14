using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //get the position of the current game object (the one the script is attached to)
        Vector3 pos = transform.position;

        //calculate the distance travelled left and right in one frame (d = v * t) 
        //the GetAxis input is either +1, 0 or -1 (gives it direction)
        //the += means you add the small distance moved in one frame along to the original value of x
        pos.x += moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        //similar to x-axis but along z-axis, up and down
        pos.z += moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        //alter the position of the game object to what the same as pos
        transform.position = pos;
    }
}
