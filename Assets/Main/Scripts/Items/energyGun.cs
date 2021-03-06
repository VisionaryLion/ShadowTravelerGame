﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class energyGun : MonoBehaviour {

    //item variables
    public GameObject playerGun; //reference to the player's gun
    public Transform gunArm; //reference to the where the player's gun will appear

    // Use this for initialization
    void Start () {

         

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        //only trigger if the object is tagged as player
        if (other.tag == "Player")
        {
            GameObject thePlayer = GameObject.Find("player"); //find's the player game object
            playerController playerScript = thePlayer.GetComponent<playerController>(); //
            playerScript.hasGun = true; //activates the bool for allowing the player to shoot
            Instantiate(playerGun, gunArm.transform); //instantiate the gun at the gun arm's location with 0 rotation
            Destroy(gameObject); //destroys the gun object

        }
    }
}
