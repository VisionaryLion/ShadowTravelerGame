using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour {

    public float fullHealth; //max amount of health the player can have
    public GameObject deathFX; //particles for the player's death
    public Vector3 respawnPoint; //respawn point

    float currentHealth; //player's current health, allows to track if they are alive or dead

    playerController controlMovement;

    //HUD Variables
    public Slider healthSlider;
    
    // Use this for initialization
	void Start () {
        currentHealth = fullHealth;
        controlMovement = GetComponent<playerController>();

        //HUD Initilization
        healthSlider.maxValue = fullHealth; //sets the max value of the health slider to the max health of the player
        healthSlider.value = fullHealth; //sets the starting value of the health slider to the full health of the player

        //initial player respawn point
        respawnPoint = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addDamage(float damage) {
        if (damage <= 0) return; //end the script if there's no damange
        Instantiate(deathFX, transform.position, transform.rotation); //spawns the death particle effects
        currentHealth -= damage;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0) {
            makeDead();
        }
    }

    public void makeDead() {
        Instantiate(deathFX, transform.position, transform.rotation); //spawns the death particle effects
        transform.position = respawnPoint; //moves the player to the position of the currnet checkpoint
        currentHealth = fullHealth;
        healthSlider.value = currentHealth;
        //Destroy(gameObject); //destroys the player
        //SceneManager.LoadScene("CaveTilemap"); //reloads the scene
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Checkpoint")
        {
            respawnPoint = other.transform.position; //makes the respawn point at the position of the trigger
        }
    }
}
