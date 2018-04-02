using System.Collections;
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
            Instantiate(playerGun, gunArm.transform); //instantiate the torch at the torch arm's location with 0 rotation
            Destroy(gameObject); //destroys the gun object

        }
    }
}
