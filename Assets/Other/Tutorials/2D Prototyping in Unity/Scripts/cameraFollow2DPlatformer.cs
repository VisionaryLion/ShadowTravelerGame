using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow2DPlatformer : MonoBehaviour {

    public Transform target; //what the camera is following
    public float smoothing; //dampening effect

    Vector3 offset; //the difference between the target and the camera

    //float lowY; //the lowest point the camera can go on the Y axis

	// Use this for initialization
	void Start () {
        offset = transform.position - target.position; //finds and maintains the difference in position between the camera and the target

        //lowY = transform.position.y; //assigns the lowest point of the camera to whatever the current Y axis setting of the camera is
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 targetCamPos = target.position + offset; //sets the camera position 

        transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime); //sets the camera to slowly move from it's starting position to the target position at the speed of the 'smoothing' variable times the time since the last frame

        //if (transform.position.y < lowY) transform.position = new Vector3 (transform.position.x, lowY, transform.position.z); //sets the y axis back to the lowY value if it goes below the lowY value, keeps the same x and z values

	}
}
