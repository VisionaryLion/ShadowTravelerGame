using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerHealth : MonoBehaviour {

    public float fullHealth; //max amount of health the player can have
    public GameObject deathFX; //particles for the player's death

    float currentHealth; //player's current health, allows to track if they are alive or dead

    playerController controlMovement;
    
    // Use this for initialization
	void Start () {
        currentHealth = fullHealth;

        controlMovement = GetComponent<playerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addDamage(float damage) {
        if (damage <= 0) return; //end the script if there's no damange
        currentHealth -= damage;

        if (currentHealth <= 0) {
            makeDead();
        }
    }

    public void makeDead() {
        Instantiate(deathFX, transform.position, transform.rotation); //spawns the death particle effects
        Destroy(gameObject); //destroys the player
        //SceneManager.LoadScene("TheCave"); //reloads the scene
    }
}
