using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallTorch : MonoBehaviour {
    
    //item variables
    public GameObject playerTorch; //reference to the player's torch
    public Transform torchArm; //reference to the where the player's torch will appear


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
            Instantiate(playerTorch, torchArm.transform); //instantiate the torch at the torch arm's location with 0 rotation
            Destroy(gameObject); //destroys the torch object

        }
    }
}
