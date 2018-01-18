using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileController : MonoBehaviour {

    public float rocketSpeed; //allows you to adjust the speed

    Rigidbody2D myRB; //reference to the rigidbody on the existing object, wherever the script is look for a rigidbody

	// Use this for initialization
	void Awake () {
        myRB = GetComponent<Rigidbody2D>(); //looking for the rigidbody that is existing on this object
        if(transform.localRotation.z>0) { //checks if the object has rotation
            myRB.AddForce(new Vector2(-1, 0) * rocketSpeed, ForceMode2D.Impulse); //adds an instant force to the rocket based on a negative X axis value of 1 times the speed of the rocket, making the rocket go to the left
        } else {
            myRB.AddForce(new Vector2(1, 0) * rocketSpeed, ForceMode2D.Impulse); //adds an instant force to the rocket based on a positive X axis value of 1 times the speed of the rocket, making the rocket go to the right
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //immediately stops the projectile from moving by setting its velocity to zero
    public void removeForce() {
        myRB.velocity = new Vector2(0, 0);
    }
}
