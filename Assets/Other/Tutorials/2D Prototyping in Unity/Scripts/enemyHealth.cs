using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour {

    public float enemyMaxHealth; //max health the enemy will have
    public GameObject deathFX; //particles for the enemy's death

    float currentHealth; //current value for the enemy's health

	// Use this for initialization
	void Start () {
        currentHealth = enemyMaxHealth; //setup the enemy's starting health
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // For adding damage to the enemy
    public void addDamage(float damage) {
        currentHealth -= damage; //subtract the damage value from the enemy's current health
        if (currentHealth <= 0) makeDead(); //calls the makeDead function if the enemy's health is 0 or lower
    }

    // Enemy death function - drops, death animation, etc.
    void makeDead() {
        Instantiate(deathFX, transform.position, transform.rotation); //spawns the death particle effects
        Destroy(gameObject); //deletes the gameobject
    }
}
