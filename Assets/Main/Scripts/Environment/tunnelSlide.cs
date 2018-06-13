using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tunnelSlide : MonoBehaviour {

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //damage the player as long as they stay within the trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        //only do damage if the object is tagged as player and the current time is past the time limit for when the enemy can do damage again
        if (other.tag == "Player")
        {
            increaseGravity(other.transform);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //only do damage if the object is tagged as player and the current time is past the time limit for when the enemy can do damage again
        if (other.tag == "Player")
        {
            decreaseGravity(other.transform);
        }
    }

    //function for pushing the object away
    void increaseGravity(Transform increasedObject)
    {
        Rigidbody2D increaseGravity = increasedObject.gameObject.GetComponent<Rigidbody2D>();
        increaseGravity.gravityScale = 20;
    }

    //function for pushing the object away
    void decreaseGravity(Transform decreasedObject)
    {
        Rigidbody2D decreaseGravity = decreasedObject.gameObject.GetComponent<Rigidbody2D>();
        decreaseGravity.gravityScale = 0.5f;
    }
}
