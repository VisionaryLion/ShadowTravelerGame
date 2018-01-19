using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDamage : MonoBehaviour {

    public float damage; //how much damage this enemy can do to the player
    public float damageRate; //how often this enemy can do damage to the player
    public float pushBackForce; //the force at which this enemy pushes the player back

    float nextDamage; //the time when this enemy can damage the player again
    
    // Use this for initialization
	void Start () {
        nextDamage = 0f; //the player can be damaged right away
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //damage the player as long as they stay within the trigger
    void OnTriggerEnter2D(Collider2D other) {
        //only do damage if the object is tagged as player and the current time is past the time limit for when the enemy can do damage again
        if (other.tag == "Player" && nextDamage < Time.time) {
            playerHealth thePlayerHealth = other.gameObject.GetComponent<playerHealth>(); //gets the health of the other collider, in this instance it's the player
            thePlayerHealth.addDamage(damage); //adds damage to the player based on the enemy's damage value
            nextDamage = Time.time + damageRate; //offsets how often the enemy does damage at a rate of the current time + damage rate

            pushBack(other.transform); //calls a function to apply a force on the other object's transform
        } 
    }

    //function for pushing the object away
    void pushBack(Transform pushedObject) {
        Vector2 pushDirection = new Vector2(0, (pushedObject.position.y - transform.position.y)).normalized; //pushes the object away in the exact opposite y position as the object pushing, normalized means make it between a value of 0 and 1
        pushDirection *= pushBackForce; //gives a value for the force being applied by multiplying the force value by the normalized Vector 2 above, in order to give it a value greater than 1
        Rigidbody2D pushRB = pushedObject.gameObject.GetComponent<Rigidbody2D>(); //finds the rigid body of the pushed object
        pushRB.velocity = Vector2.zero; //sets the velocity of the pushed rigid body to zero so no existing movement and existing forces will interfeer
        pushRB.AddForce(pushDirection, ForceMode2D.Impulse); //immediately adds an explosive impulse force in the direction of the pushed force
    }
}
