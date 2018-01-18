using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketHit : MonoBehaviour {

    public float weaponDamage;

    projectileController myPC; //a reference to the projectileController on the parent object 'projectile'

    public GameObject explosionEffect; //reference to the explosion particle

	// Use this for initialization
	void Awake () {
        myPC = GetComponentInParent<projectileController>(); //allows you to access the projectileController on this script
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //when the missile hits a trigger
    void OnTriggerEnter2D(Collider2D other) {
        //if the game object is on the same layer as the shootable layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Shootable")) {
            myPC.removeForce(); //calls the removeForce function
            Instantiate(explosionEffect, transform.position, transform.rotation); //creates the explosion effect
            Destroy(gameObject); //destroys this object (the missile) and not the projectile
            //if the other object has the enemy tag
            if (other.tag == "Enemy") {
                enemyHealth hurtEnemy = other.gameObject.GetComponent<enemyHealth>(); //assigns a new variable (hurtEnemy) to the enemyHealth script that references the object the script is on
                hurtEnemy.addDamage(weaponDamage); //calls the addDamage function in the enemyHealth script and assigns the damage value to the variable weaponDamage defined on the rocket
            } 
        }
            
    }

    //if the missile is going too fast we can still catch it
    void OnTriggerStay2D(Collider2D other) {
        //if the game object is on the same layer as the shootable layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Shootable"))
        {
            myPC.removeForce(); //calls the removeForce function
            Instantiate(explosionEffect, transform.position, transform.rotation); //creates the explosion effect
            Destroy(gameObject); //destroys this object (the missile) and not the projectile
            //if the other object has the enemy tag
            if (other.tag == "Enemy")
            {
                enemyHealth hurtEnemy = other.gameObject.GetComponent<enemyHealth>(); //assigns a new variable (hurtEnemy) to the enemyHealth script that references the object the script is on
                hurtEnemy.addDamage(weaponDamage); //calls the addDamage function in the enemyHealth script and assigns the damage value to the variable weaponDamage defined on the rocket
            }
        }
    }
}
